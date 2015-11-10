using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using System;

namespace QuickBooks.Net
{
    public class QuickBooksConnector : IQuickBooksConnector
    {
        private readonly IQuickBooksConfigurator _quickBooksConfigurator;

        public QuickBooksConnector(IQuickBooksConfigurator quickBooksConfigurator, String consumerKey, String consumerSecret)
        {
            _quickBooksConfigurator = quickBooksConfigurator;

            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
        }

        public QuickBooksConnector(String consumerKey, String consumerSecret)
            : this(new QuickBooksConfigurator(), consumerKey, consumerSecret)
        {
        }

        public String ConsumerKey { get; set; }

        public String ConsumerSecret { get; set; }

        public IToken GetRequestToken()
        {
            return PrepareOAuthSession().GetRequestToken();
        }

        public String GetAuthorizationLink(IToken requestToken, String callbackUrl)
        {
            return PrepareOAuthSession().GetUserAuthorizationUrlForToken(requestToken, callbackUrl);
        }

        public IToken VerifyAccessToken(IToken requestToken, String verifier, String realmId)
        {
            var accessToken = PrepareOAuthSession().ExchangeRequestTokenForAccessToken(requestToken, verifier);
            accessToken.Realm = realmId;

            return accessToken;
        }

        #region Private

        private OAuthSession PrepareOAuthSession()
        {
            var consumerContext = new OAuthConsumerContext
            {
                ConsumerKey = ConsumerKey,
                ConsumerSecret = ConsumerSecret,
                SignatureMethod = SignatureMethod.HmacSha1
            };

            var session = new OAuthSession(consumerContext, _quickBooksConfigurator.RequestTokenUrl, _quickBooksConfigurator.UserAuthorizeUrl, _quickBooksConfigurator.AccessTokenRequestUrl);

            return session;
        }

        #endregion Private
    }
}
