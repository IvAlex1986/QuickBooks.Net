using System.Web;
using System.Web.Mvc;

namespace QuickBooks.Net.Mvc.Sample
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
