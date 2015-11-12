using DevDefined.OAuth.Framework;
using System;

namespace QuickBooks.Net
{
    public interface IQuickBooksConnector
    {
        String ConsumerKey { get; }

        String ConsumerSecret { get; }

        IToken GetRequestToken();

        String GetAuthorizationLink(IToken requestToken, String callbackUrl);

        IToken VerifyAccessToken(IToken requestToken, String verifier, String realmId);

        void DisconnectToken(IToken accessToken);

        IToken ReconnectToken(IToken accessToken, out DateTime createTokenDateTime);
    }
}
