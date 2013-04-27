using System;
using System.Net;
using System.Security.Authentication;
using System.ServiceModel.Web;
using System.Web;

namespace WcfExt.Authentication
{
    /// <summary>
    /// Custom authorization manager which provide Basic Authentication
    /// </summary>
    public class BasicWebHttpAuthorizationManager : CustomAuthorizationManager
    {
        public override Type UserCredentialIssuerType
        {
            get
            {
                return typeof(BasicWebHttpCredentialIssuer);
            }
            protected internal set
            {
                // Do nothing
            }
        }

        protected override bool CheckAccessCore(System.ServiceModel.OperationContext operationContext)
        {            
            try
            {
                var result = base.CheckAccessCore(operationContext);
                return result;
            }
            catch (AuthenticationException)
            {
                HttpContext.Current.Response.Headers.Add("WWW-Authenticate", "Basic");
                throw new WebFaultException(HttpStatusCode.Unauthorized);
            }
        }
    }
}
