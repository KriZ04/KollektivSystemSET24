using KollektivSystem.ApiService.Services;
using Microsoft.Extensions.Logging;

namespace KollektivSystem.ApiService.Infrastructure.Logging
{
    internal static partial class TransitLineLogMessages
    {
        [LoggerMessage
            (Level = LogLevel.Information,
            Message = "Transit line {lineId} ({lineName}) created successfully.")]
        internal static partial void LogTransitLineCreated(this ILogger<TransitLineService> logger, int lineId, string lineName);

        [LoggerMessage
        (Level = LogLevel.Information,
        Message = "Transit line {lineId} ({lineName}) creation failed.")]
        internal static partial void LogTransitLineCreationFailed(this ILogger<TransitLineService> logger, int lineId, string lineName);

        [LoggerMessage
            (Level = LogLevel.Warning,
            Message = "Transit line with ID {lineId} not found.")]
        internal static partial void LogTransitLineNotFound(this ILogger<TransitLineService> logger, int lineId);

        [LoggerMessage
            (Level = LogLevel.Error,
            Message = "Error updating transit line {lineId}: {errorMessage}")]
        internal static partial void LogTransitLineUpdateFailed(this ILogger<TransitLineService> logger, int lineId, string errorMessage);


    }
}

