using System;
using System.Web.Mvc;

namespace QuickBooks.Net.Mvc.Sample.Extensions
{
    public static class UrlHelperExtension
    {
        public static string AbsoluteAction(this UrlHelper urlHelper, String actionName, String controllerName = null, Object routeValues = null)
        {
            var url = urlHelper.RequestContext.HttpContext.Request.Url;
            var scheme = (url != null) ? url.Scheme : "http";

            return urlHelper.Action(actionName, controllerName, routeValues, scheme);
        }
    }
}