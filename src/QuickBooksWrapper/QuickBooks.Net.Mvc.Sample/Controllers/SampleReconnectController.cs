using DevDefined.OAuth.Framework;
using QuickBooks.Net.Mvc.Sample.Extensions;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace QuickBooks.Net.Mvc.Sample.Controllers
{
    public class SampleReconnectController : BaseController
    {
        private readonly IQuickBooksConnector _quickBooksConnector;

        public SampleReconnectController()
        {
            var consumerKey = ConfigurationManager.AppSettings.Get("ConsumerKey");
            var consumerSecret = ConfigurationManager.AppSettings.Get("ConsumerSecret");

            _quickBooksConnector = new QuickBooksConnector(consumerKey, consumerSecret);
        }

        public ActionResult DisconnectToken()
        {
            var model = RunAction((acсessToken) =>
            {
                _quickBooksConnector.DisconnectToken(acсessToken);
            });

            return View("View", (Object)model);
        }

        public ActionResult ReconnectToken()
        {
            var model = RunAction((acсessToken) =>
            {
                DateTime createTokenTimeUtc;
                var token = _quickBooksConnector.ReconnectToken(acсessToken, out createTokenTimeUtc);
                SessionState.SaveValue("AccessToken", token);
                SessionState.SaveValue("СreateTokenTimeUtc", createTokenTimeUtc);
            });

            return View("View", (Object)model);
        }

        #region Private

        private String RunAction(Action<IToken> action)
        {
            String result;

            try
            {
                var accessToken = SessionState.GetValue<IToken>("AccessToken");
                action(accessToken);

                result = "Token Disconnected Sucessfull.";
            }
            catch (QuickBooksException exception)
            {
                result = String.Format("Disconnect Failed. Error Code: {0}; Error Message: {1}", exception.ErrorCode, exception.Message);
            }
            catch (Exception exception)
            {
                result = String.Format("Disconnect Failed. Unhandled Exception: {0}", exception.Message);
            }

            return result;
        }

        #endregion Private
    }
}