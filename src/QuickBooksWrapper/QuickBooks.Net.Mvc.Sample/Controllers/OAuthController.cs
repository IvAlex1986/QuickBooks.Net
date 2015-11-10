using DevDefined.OAuth.Framework;
using QuickBooks.Net.Mvc.Sample.Helpers;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace QuickBooks.Net.Mvc.Sample.Controllers
{
    public class OAuthController : Controller
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
            SessionHelper.SaveValue("RequestToken", requestToken);

            var callbackUrl = new UrlHelper(ControllerContext.RequestContext).Action("AccessToken", null, null, Request.Url.Scheme);
            var authorizationUrl = _quickBooksConnector.GetAuthorizationLink(requestToken, callbackUrl);

            return Redirect(authorizationUrl);
        }

        // ReSharper disable InconsistentNaming
        public ActionResult AccessToken(String oauth_token, String oauth_verifier, String realmId)
        {
            var requestToken = SessionHelper.GetValue<IToken>("RequestToken");

            var accessToken = _quickBooksConnector.VerifyAccessToken(requestToken, oauth_verifier, realmId);
            SessionHelper.SaveValue("AccessToken", accessToken);

            return RedirectToAction("ClosePage");
        }

        public ActionResult ClosePage()
        {
            return View();
        }
    }
}