using KollektivSystem.ApiService.Services;
using Microsoft.Extensions.Logging;

namespace KollektivSystem.ApiService.Infrastructure.Logging
{
    internal static partial class TransitLineLogMessages
    {
        private const int Base = LogAreas.TransitLine;

        [LoggerMessage(
            EventId = Base + 100,
            Level = LogLevel.Information,
            Message = "Transit line {lineId} ({lineName}) created successfully.")]
        internal static partial void LogTransitLineCreated(this ILogger<TransitLineService> logger, int lineId, string lineName);

        [LoggerMessage(
            EventId = Base + 101,
            Level = LogLevel.Warning,
            Message = "Transit line with ID {lineId} not found.")]
        internal static partial void LogTransitLineNotFound(this ILogger<TransitLineService> logger, int lineId);

        [LoggerMessage(
            EventId = Base + 102,
            Level = LogLevel.Error,
            Message = "Error updating transit line {lineId}: {errorMessage}")]
        internal static partial void LogTransitLineUpdateFailed(this ILogger<TransitLineService> logger, int lineId, string errorMessage);


    }
}

