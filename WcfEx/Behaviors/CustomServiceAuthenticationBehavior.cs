using System;
using WcfEx.Authentication;

namespace WcfEx.Behaviors
{
    public class CustomServiceAuthenticationBehavior : ServiceBehaviorBase
    {
        public AuthenticationProvider Provider { get; set; }
        public string MembershipProviderName { get; set; }
        public Type ValidatorType { get; set; }
        public Type UserCredentialIssuerType { get; set; }

        public override void ApplyDispatchBehavior(System.ServiceModel.Description.ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            var authManager = serviceHostBase.Authorization.ServiceAuthorizationManager as CustomAuthorizationManager;
            if (authManager == null) return;

            authManager.Provider = Provider;
            authManager.MembershipProviderName = MembershipProviderName;
            authManager.ValidatorType = ValidatorType;
            authManager.UserCredentialIssuerType = UserCredentialIssuerType;
        }
    }
}
