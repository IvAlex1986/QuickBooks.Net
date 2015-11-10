using System;
using System.Web;
using System.Web.SessionState;

namespace QuickBooks.Net.Mvc.Sample.Helpers
{
    public static class SessionHelper
    {
        public static HttpSessionState Session
        {
            get
            {
                return HttpContext.Current.Session;
            }
        }

        public static void SaveValue(String key, Object value)
        {
            Session[key] = value;
        }

        public static Object GetValue(String key)
        {
            return Session[key];
        }

        public static T GetValue<T>(String key)
        {
            return (T)GetValue(key);
        }
    }
}