using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using System;
using System.IO;
using System.Text;
using System.Xml;

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

        public void DisconnectToken(IToken accessToken)
        {
            ProcessConsumerRequest(accessToken, _quickBooksConfigurator.DisconnectTokenUrl);
        }

        public IToken ReconnectToken(IToken accessToken, out DateTime createTokenTimeUtc)
        {
            var body = ProcessConsumerRequest(accessToken, _quickBooksConfigurator.ReconnectTokenUrl);

            var token = new TokenBase
            {
                Token = GetElementValueFromResponse<String>(body, "OAuthToken"),
                TokenSecret = GetElementValueFromResponse<String>(body, "OAuthTokenSecret"),
                Realm = accessToken.Realm,
                ConsumerKey = accessToken.ConsumerKey
            };

            createTokenTimeUtc = GetElementValueFromResponse<DateTime>(body, "ServerTime").ToUniversalTime();

            return token;
        }

        #region Private

        private IOAuthSession PrepareOAuthSession()
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

        private IConsumerRequest PrepareConsumerRequest(IToken accessToken)
        {
            var session = PrepareOAuthSession();

            session.AccessToken = accessToken;
            session.ConsumerContext.UseHeaderForOAuthParameters = true;

            return session.Request();
        }

        private String ProcessConsumerRequest(IToken accessToken, String url)
        {
            var request = PrepareConsumerRequest(accessToken);
            request = request.Get().ForUrl(url);

            var body = GetConsumerResponseBody(request);

            var errorCode = GetElementValueFromResponse<Int32>(body, "ErrorCode");
            if (errorCode != 0)
            {
                var message = GetElementValueFromResponse<String>(body, "ErrorMessage");
                throw new QuickBooksException(errorCode, message);
            }

            return body;
        }

        private static String GetConsumerResponseBody(IConsumerRequest request)
        {
            var body = String.Empty;

            using (var response = request.ToWebResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        var reader = new StreamReader(stream, Encoding.UTF8);
                        body = reader.ReadToEnd();
                    }
                }
            }

            return body;
        }

        private static T GetElementValueFromResponse<T>(String body, String element) where T : IConvertible
        {
            String value = String.Empty;

            XmlDocument document = new XmlDocument();
            document.LoadXml(body);

            XmlNode node = document.GetElementsByTagName(element).Item(0);
            if (node != null)
            {
                value = node.InnerText;
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }

        #endregion Private
    }
}
