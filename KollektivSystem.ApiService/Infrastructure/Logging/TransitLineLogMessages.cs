using Microsoft.Extensions.Logging;

namespace KollektivSystem.ApiService.Infrastructure.Logging


{
    public static partial class TransitLineLogMessages
    {
        [LoggerMessage(Level = LogLevel.Information,
            Message = "Transit line {lineId} ({lineName}) created successfully.")]
        static partial void LogTransitLineCreated(this ILogger logger, int lineId, string lineName);

        [LoggerMessage(Level = LogLevel.Warning,
            Message = "Transit line with ID {lineId} not found.")]
        static partial void LogTransitLineNotFound(this ILogger logger, int lineId);

        [LoggerMessage(Level = LogLevel.Error,
            Message = "Error updating transit line {lineId}: {errorMessage}")]
        static partial void LogTransitLineUpdateFailed(this ILogger logger, int lineId, string errorMessage);
    }
 }
