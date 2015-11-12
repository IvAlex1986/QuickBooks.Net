using System.Web.Mvc;
using System.Web.SessionState;

namespace QuickBooks.Net.Mvc.Sample.Controllers
{
    public class BaseController : Controller
    {
        public HttpSessionState SessionState
        {
            get
            {
                return System.Web.HttpContext.Current.Session;
            }
        }
    }
}