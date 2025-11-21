namespace KollektivSystem.ApiService.Infrastructure.Logging.Endpoints;

public static partial class UserLogMessages
{
    private const int Base = LogAreas.Users;

    [LoggerMessage(
        EventId = Base + 100,
        Level = LogLevel.Information, 
        Message = "User {userId} created, using provider {provider} and subject {sub}.")]
    public static partial void LogUserCreated(this ILogger logger, Guid userId, string provider, string sub);

}
