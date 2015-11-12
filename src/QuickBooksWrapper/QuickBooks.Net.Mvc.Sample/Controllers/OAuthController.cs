using DevDefined.OAuth.Framework;
using QuickBooks.Net.Mvc.Sample.Extensions;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace QuickBooks.Net.Mvc.Sample.Controllers
{
    public class OAuthController : BaseController
    {
        private readonly IQuickBooksConnector _quickBooksConnector;

        public OAuthController()
        {
            var consumerKey = ConfigurationManager.AppSettings.Get("ConsumerKey");
            var consumerSecret = ConfigurationManager.AppSettings.Get("ConsumerSecret");

            _quickBooksConnector = new QuickBooksConnector(consumerKey, consumerSecret);
        }

        public ActionResult RequestToken()
        {
            var requestToken = _quickBooksConnector.GetRequestToken();
            SessionState.SaveValue("RequestToken", requestToken);

            var callbackUrl = new UrlHelper(ControllerContext.RequestContext).AbsoluteAction("AccessToken");
            var authorizationUrl = _quickBooksConnector.GetAuthorizationLink(requestToken, callbackUrl);

            return Redirect(authorizationUrl);
        }

        // ReSharper disable InconsistentNaming
        public ActionResult AccessToken(String oauth_token, String oauth_verifier, String realmId)
        {
            var requestToken = SessionState.GetValue<IToken>("RequestToken");

            var accessToken = _quickBooksConnector.VerifyAccessToken(requestToken, oauth_verifier, realmId);
            SessionState.SaveValue("AccessToken", accessToken);

            return RedirectToAction("ClosePage");
        }

        public ActionResult ClosePage()
        {
            return View();
        }
    }
}