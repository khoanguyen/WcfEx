using System;
using System.Configuration;
using System.ServiceModel.Configuration;
using WcfEx.Authentication;
using WcfEx.Behaviors;

namespace WcfEx.Configuration
{
    /// <summary>
    /// Extension Element for configuring Custom Authentication
    /// </summary>
    public class CustomServiceAuthenticationExtensionElement : BehaviorExtensionElement
    {
        private const string ProviderKey = "provider";
        private const string MembershipProviderNameKey = "membershipProviderName";
        private const string ValidatorKey = "usernamePasswordValidatorType";
        private const string UserCredentialIssuerKey = "userCredentialIssuerType";

        [ConfigurationProperty(ProviderKey)]
        public AuthenticationProvider Provider
        {
            get { return (AuthenticationProvider)this[ProviderKey]; }
            set { this[ProviderKey] = value; }
        }

        [ConfigurationProperty(MembershipProviderNameKey)]
        public string MembershipProviderName
        {
            get { return (string)this[MembershipProviderNameKey]; }
            set { this[MembershipProviderNameKey] = value; }
        }

        [ConfigurationProperty(ValidatorKey)]
        public string UsernamePasswordValidatorType
        {
            get { return (string)this[ValidatorKey]; }
            set { this[ValidatorKey] = value; }
        }

        [ConfigurationProperty(UserCredentialIssuerKey)]
        public string UserCredentialIssuerType
        {
            get { return (string)this[UserCredentialIssuerKey]; }
            set { this[UserCredentialIssuerKey] = value; }
        }

        protected override object CreateBehavior()
        {
            Type validatorType = null;
            if (!string.IsNullOrWhiteSpace(UsernamePasswordValidatorType))
                validatorType = Type.GetType(UsernamePasswordValidatorType);

            Type issuerType = null;
            if (!string.IsNullOrWhiteSpace(UserCredentialIssuerType))
                issuerType = Type.GetType(UserCredentialIssuerType);

            return new CustomServiceAuthenticationBehavior
                {
                    Provider = Provider,
                    MembershipProviderName = MembershipProviderName,
                    ValidatorType = validatorType,
                    UserCredentialIssuerType = issuerType
                };
        }

        public override Type BehaviorType
        {
            get { return typeof(CustomServiceAuthenticationBehavior); }
        }
    }
}
