using System;

namespace QuickBooks.Net
{
    public interface IQuickBooksConfigurator
    {
        String RequestTokenUrl { get; }

        String UserAuthorizeUrl { get; }

        String AccessTokenRequestUrl { get; }

        String DisconnectTokenUrl { get; }

        String ReconnectTokenUrl { get; }

        Int32 TokenExpirationInDays { get; }

        Int32 TokenCanReconnectInDays { get; }
    }
}
