using System;
using System.IdentityModel.Selectors;
using System.Security.Authentication;
using System.ServiceModel;
using System.Web.Security;

namespace WcfEx.Authentication
{
    /// <summary>
    /// Authorization Manager which provide custom Authentication
    /// </summary>
    public class CustomAuthorizationManager : ServiceAuthorizationManager
    {        
        public virtual AuthenticationProvider Provider { get; protected internal set; }
        public virtual Type ValidatorType { get; protected internal set; }
        public virtual string MembershipProviderName { get; protected internal set; }
        public virtual Type UserCredentialIssuerType{ get; protected internal set; }

        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            var message = operationContext.RequestContext.RequestMessage;
            var issuer = GetUserCredentialIssuer(UserCredentialIssuerType);
            var credential = issuer.GetUserCredential(message);
            if (credential == null)
                throw new AuthenticationException("Client credential was not provided");

            // Validate client credential with MembershipProvider or UserNamePasswordValidator
            var authResult = Provider == AuthenticationProvider.Membership
                                 ? MembershipAuthenticate(MembershipProviderName, credential)
                                 : ValidatorAuthenticate(ValidatorType, credential);

            if (!authResult)
                throw new AuthenticationException("Invalid client credential");

            return base.CheckAccessCore(operationContext);
        }

        /// <summary>
        /// Validate client credential with UserNamePasswordValidator
        /// </summary>
        /// <param name="validatorType"></param>
        /// <param name="credential"></param>
        /// <returns></returns>
        private static bool ValidatorAuthenticate(Type validatorType, UserCredential credential)
        {
            if (validatorType == null)
                throw new Exception("Custom validator is not set");
            if (!typeof(UserNamePasswordValidator).IsAssignableFrom(validatorType))
                throw new Exception(
                    "Validator type must be derived class of System.IdentityModel.Selectors.UserNamePasswordValidator");
            var validator = Activator.CreateInstance(validatorType) as UserNamePasswordValidator;

            if (validator != null) validator.Validate(credential.Username, credential.Password);
            return true;
        }

        /// <summary>
        /// Validate client credential with MembershipProvider
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="credential"></param>
        /// <returns></returns>
        private static bool MembershipAuthenticate(string providerName, UserCredential credential)
        {
            var provider = Membership.Providers[providerName];
            if (provider == null)
                throw new Exception("Membership Provider not found");

            return provider.ValidateUser(credential.Username, credential.Password);
        }

        /// <summary>
        /// Get Credential issuer instance
        /// </summary>
        /// <param name="issuerType"></param>
        /// <returns></returns>
        private static IUserCredentialIssuer GetUserCredentialIssuer(Type issuerType)
        {
            if (issuerType == null)
                throw new Exception("User credential issuer is not set");
            if (!typeof(IUserCredentialIssuer).IsAssignableFrom(issuerType))
                throw new Exception(
                    "User Credential issuer type must implement WcfEx.Authentication.IUserCredentialIssuer");
            var issuer = Activator.CreateInstance(issuerType) as IUserCredentialIssuer;
            return issuer;
        }
        
    }
}
