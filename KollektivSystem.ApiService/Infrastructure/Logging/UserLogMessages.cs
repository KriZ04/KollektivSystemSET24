using Microsoft.Extensions.Logging;

namespace KollektivSystem.ApiService.Infrastructure.Logging
{
    public static partial class UserLogMessages
    {
        [LoggerMessage(Level = LogLevel.Information, Message = "User {userId} created, using provider {provider} and subject {sub}.")]
        static partial void LogUserCreated(this ILogger logger, Guid userId, string provider, string sub);

    }
}
