using System;
using System.Web.SessionState;

namespace QuickBooks.Net.Mvc.Sample.Extensions
{
    public static class SessionExtension
    {
        public static void SaveValue(this HttpSessionState sessionState, String key, Object value)
        {
            sessionState[key] = value;
        }

        public static Object GetValue(this HttpSessionState sessionState, String key)
        {
            return sessionState[key];
        }

        public static T GetValue<T>(this HttpSessionState sessionState, String key)
        {
            return (T)GetValue(sessionState, key);
        }
    }
}