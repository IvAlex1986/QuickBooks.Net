using System.Web.Mvc;

namespace QuickBooks.Net.Mvc.Sample.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}