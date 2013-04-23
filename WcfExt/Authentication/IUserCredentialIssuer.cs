using System.ServiceModel.Channels;

namespace WcfExt.Authentication
{
    /// <summary>
    /// Credential Issuer provide client's Credential from incoming message
    /// </summary>
    public interface IUserCredentialIssuer
    {
        /// <summary>
        /// Get Client's credential from given message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        UserCredential GetUserCredential(Message message);
    }
}
