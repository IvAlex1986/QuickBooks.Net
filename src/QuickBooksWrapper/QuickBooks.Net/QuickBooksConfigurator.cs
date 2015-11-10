using System;
using System.Configuration;

namespace QuickBooks.Net
{
    public class QuickBooksConfigurator : IQuickBooksConfigurator
    {
        public string RequestTokenUrl
        {
            get
            {
                return GetConfigurationSettingValue("RequestTokenUrl", "https://oauth.intuit.com/oauth/v1/get_request_token");
            }
        }

        public string UserAuthorizeUrl
        {
            get
            {
                return GetConfigurationSettingValue("UserAuthorizeUrl", "https://appcenter.intuit.com/Connect/Begin");
            }
        }

        public string AccessTokenRequestUrl
        {
            get
            {
                return GetConfigurationSettingValue("AccessTokenRequestUrl", "https://oauth.intuit.com/oauth/v1/get_access_token");
            }
        }

        public string DisconnectTokenUrl
        {
            get
            {
                return GetConfigurationSettingValue("DisconnectTokenUrl", "https://appcenter.intuit.com/api/v1/connection/disconnect");
            }
        }

        public string ReconnectTokenUrl
        {
            get
            {
                return GetConfigurationSettingValue("ReconnectTokenUrl", "https://appcenter.intuit.com/api/v1/connection/reconnect");
            }
        }

        public int TokenExpirationInDays
        {
            get
            {
                return Int32.Parse(GetConfigurationSettingValue("TokenExpirationInDays", "180"));
            }
        }

        public int TokenCanReconnectInDays
        {
            get
            {
                return Int32.Parse(GetConfigurationSettingValue("TokenCanReconnectInDays", "30"));
            }
        }

        #region Private

        private static String GetConfigurationSettingValue(String key, String defaultValue = "")
        {
            return ConfigurationManager.AppSettings.Get(key) ?? defaultValue;
        }

        #endregion Private
    }
}
