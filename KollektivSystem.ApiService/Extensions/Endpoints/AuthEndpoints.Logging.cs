namespace KollektivSystem.ApiService.Infrastructure.Logging
{
    public static partial class AuthEndpointsLogMessages
    {
        // Login
        [LoggerMessage(EventId = 1000, Level = LogLevel.Warning, Message = "Login failed: missing returnUrl.")]
        public static partial void LoginMissingReturnUrl(this ILogger logger);

        [LoggerMessage(EventId = 1001, Level = LogLevel.Warning, Message = "Login failed: invalid returnUrl '{returnUrl}'.")]
        public static partial void LoginInvalidReturnUrl(this ILogger logger, string returnUrl);

        [LoggerMessage(EventId = 1002, Level = LogLevel.Information, Message = "Login request accepted. Redirecting to authorization provider. State={state} ReturnUrl={returnUrl}")]
        public static partial void LoginRedirecting(this ILogger logger, string state, string returnUrl);
        

        // Callback
        [LoggerMessage(EventId = 1010, Level = LogLevel.Warning, Message = "Callback failed: missing 'code' or 'state'.")]
        public static partial void CallbackMissingCodeOrState(this ILogger logger);

        [LoggerMessage(EventId = 1011, Level = LogLevel.Warning, Message = "Callback failed: invalid state '{state}'.")]
        public static partial void CallbackInvalidState(this ILogger logger, string state);

        [LoggerMessage(EventId = 1012, Level = LogLevel.Warning, Message = "Callback failed: missing 'returnUrl'.")]
        public static partial void CallbackMissingReturnUrl(this ILogger logger);

        [LoggerMessage(EventId = 1013, Level = LogLevel.Warning, Message = "Callback failed: invalid returnUrl '{returnUrl}' for state '{state}'.")]
        public static partial void CallbackInvalidReturnUrl(this ILogger logger, string state, string returnUrl);

        [LoggerMessage(EventId = 1014, Level = LogLevel.Information, Message = "Callback started for state '{state}'. Exchanging code with provider '{provider}'.")]
        public static partial void CallbackExchangingCode(this ILogger logger, string state, string provider);

        [LoggerMessage(EventId = 1015, Level = LogLevel.Information, Message = "Callback succeeded. User {userId} signed in with provider '{provider}' and subject '{sub}'.")]
        public static partial void CallbackUserSignedIn(this ILogger logger, Guid userId, string provider, string sub);

        [LoggerMessage(EventId = 1016, Level = LogLevel.Information, Message = "Callback completed. Redirecting to returnUrl '{returnUrl}'.")]
        public static partial void CallbackRedirecting(this ILogger logger, string returnUrl);

        // Refresh
        [LoggerMessage(EventId = 1020, Level = LogLevel.Warning, Message = "Refresh failed: invalid or expired refresh token.")]
        public static partial void RefreshFailed(this ILogger logger);

        [LoggerMessage(EventId = 1021, Level = LogLevel.Information, Message = "Refresh succeeded: issued new access and refresh tokens.")]
        public static partial void RefreshSucceeded(this ILogger logger);
    }
}
