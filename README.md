# QuickBooks.Net
**QuickBooks.Net** is .NET API wrapper for QuickBooks.
<br />
The official documentation of using API is [here](https://www.matecat.com/api/docs).
<br />
Also this repository can be used as C# MVC example for QuickBooks OAuth.
<br />
Details about OAuth look by this [link](https://developer.intuit.com/docs/0050_quickbooks_api).

# Example

Example of using **FindAll** method:
```c#
using DevDefined.OAuth.Framework;
using Intuit.Ipp.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace QuickBooks.Net.Example
{
    public class ExampleController: Controller
    {
        private readonly IQuickBooksAdapter _quickBooksAdapter;

        public ExampleController()
        {
            IToken accessToken = (IToken)System.Web.HttpContext.Current.Session["AccessToken"];

            String baseUrl = ConfigurationManager.AppSettings.Get("BaseUrl");
            String appToken = ConfigurationManager.AppSettings.Get("AppToken");
            String consumerKey = ConfigurationManager.AppSettings.Get("ConsumerKey");
            String consumerSecret = ConfigurationManager.AppSettings.Get("ConsumerSecret");

            _quickBooksAdapter = new QuickBooksAdapter(accessToken, appToken, consumerKey, consumerSecret, baseUrl);
        }

        public ActionResult FindAllCustomers()
        {
            List<Customer> customers = _quickBooksAdapter.FindAll<Customer>().ToList();
            return View("CustomersView", customers);
        }
    }
}
```

Example of using **Add** method:
```c#
public ActionResult AddTimeActivity()
{
    Customer customer = _quickBooksAdapter.FindAll<Customer>().First();
    Vendor vendor = _quickBooksAdapter.FindAll<Vendor>().First();
    Item item = _quickBooksAdapter.FindAll<Item>().First();

    TimeActivity timeActivity = new TimeActivity
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

    return View("TimeActivityView", timeActivity);
}
```

# License
This project is licensed under the [MIT license](https://github.com/IvAlex1986/QuickBooks.Net/blob/master/LICENSE).
