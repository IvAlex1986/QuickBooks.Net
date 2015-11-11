using DevDefined.OAuth.Framework;
using Intuit.Ipp.Data;
using QuickBooks.Net.Mvc.Sample.Helpers;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace QuickBooks.Net.Mvc.Sample.Controllers
{
    public class SampleApiController : Controller
    {
        private readonly IQuickBooksAdapter _quickBooksAdapter;

        public SampleApiController()
        {
            var accessToken = SessionHelper.GetValue<IToken>("AccessToken");

            var baseUrl = ConfigurationManager.AppSettings.Get("BaseUrl");
            var appToken = ConfigurationManager.AppSettings.Get("AppToken");
            var consumerKey = ConfigurationManager.AppSettings.Get("ConsumerKey");
            var consumerSecret = ConfigurationManager.AppSettings.Get("ConsumerSecret");

            _quickBooksAdapter = new QuickBooksAdapter(accessToken, appToken, consumerKey, consumerSecret, baseUrl);
        }

        public ActionResult FindAllCustomers()
        {
            var customers = _quickBooksAdapter.FindAll<Customer>().ToList();

            var model = customers.Select(n => String.Format("Customer DisplayName: {0}", n.DisplayName)).ToList();
            return View("View", model);
        }

        public ActionResult FindAllEmployees()
        {
            var employees = _quickBooksAdapter.FindAll<Employee>().ToList();

            var model = employees.Select(n => String.Format("Employee DisplayName: {0}", n.DisplayName)).ToList();
            return View("View", model);
        }

        public ActionResult FindAllItems()
        {
            var items = _quickBooksAdapter.FindAll<Item>().ToList();

            var model = items.Select(n => String.Format("Item Name: {0}", n.Name)).ToList();
            return View("View", model);
        }

        public ActionResult FindAllTimeActivities()
        {
            var timeActivities = _quickBooksAdapter.FindAll<TimeActivity>().ToList();

            var model = timeActivities.Select(n => String.Format("TimeActivity Description: {0}", n.Description)).ToList();
            return View("View", model);
        }
    }
}