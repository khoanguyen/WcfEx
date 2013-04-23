using System;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;

namespace WcfEx.Authentication
{
    /// <summary>
    /// Client credential issuer for Basic Authentication. 
    /// This issuer is used by BasicAuthorizationManager
    /// </summary>
    public class BasicAuthCredentialIssuer : IUserCredentialIssuer
    {
        private const string HttpRequestPropertyKey = "httpRequest";
        private const string AuthorizationHeader = "Authorization";

        public UserCredential GetUserCredential(Message message)
        {            
            var httpRequest = message.Properties[HttpRequestPropertyKey] as HttpRequestMessageProperty;
            if (httpRequest == null) return null;
            if (!httpRequest.Headers.AllKeys.Contains(AuthorizationHeader)) return null;
            var authorizationHeader = httpRequest.Headers[AuthorizationHeader].Trim();

            if (!authorizationHeader.StartsWith("Basic")) return null;
            var tokens = authorizationHeader.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length < 2) return null;
            var credentialText = Encoding.UTF8.GetString(Convert.FromBase64String(tokens[1]));

            if (string.IsNullOrWhiteSpace(credentialText)) return null;
            var credentialTokens = credentialText.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

            if (credentialTokens.Length == 0) return null;
            return new UserCredential
                {
                    Username = credentialTokens[0],
                    Password = credentialTokens.Length > 1 ? credentialTokens[1] : string.Empty
                };
        }
    }
}
