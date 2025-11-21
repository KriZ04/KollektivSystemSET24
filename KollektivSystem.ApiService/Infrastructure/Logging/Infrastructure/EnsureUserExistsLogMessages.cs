namespace KollektivSystem.ApiService.Infrastructure.Logging.Infrastructure;

internal static partial class EnsureUserExistsLogMessages
{
    private const int Base = LogAreas.Infrastructure;
    [LoggerMessage(EventId = Base + 0, Level = LogLevel.Warning, Message = "Authenticated principal has invalid or missing NameIdentifier claim.")]
    public static partial void InvalidOrMissingNameIdClaim(this ILogger logger);

    [LoggerMessage(EventId = Base + 1, Level = LogLevel.Information, Message = "Authenticated user with id {UserId} does not exist in database.")]
    public static partial void UserDoesNotExist(this ILogger logger, Guid userId);
}
