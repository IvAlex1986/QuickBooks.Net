using DevDefined.OAuth.Framework;
using Intuit.Ipp.Data;
using QuickBooks.Net.Mvc.Sample.Extensions;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace QuickBooks.Net.Mvc.Sample.Controllers
{
    public class SampleApiController : BaseController
    {
        private readonly IQuickBooksAdapter _quickBooksAdapter;

        public SampleApiController()
        {
            var accessToken = SessionState.GetValue<IToken>("AccessToken");

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

        public ActionResult AddTimeActivity()
        {
            var customer = _quickBooksAdapter.FindAll<Customer>().First();
            var vendor = _quickBooksAdapter.FindAll<Vendor>().First();
            var item = _quickBooksAdapter.FindAll<Item>().First();

            var timeActivity = new TimeActivity
            {
                CustomerRef = new ReferenceType
                {
                    Value = customer.Id,
                    name = customer.DisplayName
                },
                ItemRef = new ReferenceType
                {
                    Value = item.Id,
                    name = item.Name
                },
                AnyIntuitObject = new ReferenceType
                {
                    Value = vendor.Id,
                    name = vendor.DisplayName
                },
                ItemElementName = ItemChoiceType5.VendorRef,
                NameOf = TimeActivityTypeEnum.Vendor,
                NameOfSpecified = true,
                TxnDate = DateTime.UtcNow.Date,
                TxnDateSpecified = true,
                BillableStatus = BillableStatusEnum.NotBillable,
                BillableStatusSpecified = true,
                Taxable = false,
                TaxableSpecified = true,
                HourlyRate = 35.0m,
                HourlyRateSpecified = true,
                Hours = 8,
                HoursSpecified = true,
                Minutes = 0,
                MinutesSpecified = true,

                Description = String.Format("New TimeActivity entity is created from QuickBooks.Net wrapper in {0:u}", DateTime.Now)
            };

            _quickBooksAdapter.Add(timeActivity);

            return FindAllTimeActivities();
        }
    }
}