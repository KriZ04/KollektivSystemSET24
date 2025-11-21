namespace KollektivSystem.ApiService.Infrastructure.Logging.Services;

internal static partial class TransitLineServiceLogMessages
{
    private const int Base = LogAreas.TransitLine;
    [LoggerMessage(
        EventId = Base + 100,
        Level = LogLevel.Information,
        Message = "Transit line {lineId} ({lineName}) created successfully.")]
    public static partial void LogTransitLineCreated(this ILogger logger, int lineId, string lineName);

    [LoggerMessage(
        EventId = Base + 101,
        Level = LogLevel.Error,
        Message = "Error creating transit line {lineId} ({lineName}): {errorMessage}")]
    public static partial void LogTransitLineCreationFailed(this ILogger logger, int lineId, string lineName, string errorMessage);

    [LoggerMessage(
        EventId = Base + 102,
        Level = LogLevel.Warning,
        Message = "Transit line with ID {lineId} not found.")]
    public static partial void LogTransitLineNotFound(this ILogger logger, int lineId);

    [LoggerMessage(
        EventId = Base + 103,
        Level = LogLevel.Information,
        Message = "Transit line {lineId} ({lineName}) updated successfully.")]
    public static partial void LogTransitLineUpdated(this ILogger logger, int lineId, string lineName);

    [LoggerMessage(
        EventId = Base + 104,
        Level = LogLevel.Error,
        Message = "Error updating transit line {lineId}: {errorMessage}")]
    public static partial void LogTransitLineUpdateFailed(this ILogger logger, int lineId, string errorMessage);

    [LoggerMessage(
        EventId = Base + 105,
        Level = LogLevel.Information,
        Message = "Transit line {lineId} ({lineName}) deleted successfully.")]
    public static partial void LogTransitLineDeleted(this ILogger logger, int lineId, string lineName);

    [LoggerMessage(
        EventId = Base + 106,
        Level = LogLevel.Error,
        Message = "Error deleting transit line {lineId}: {errorMessage}")]
    public static partial void LogTransitLineDeleteFailed(this ILogger logger, int lineId, string errorMessage);
}
