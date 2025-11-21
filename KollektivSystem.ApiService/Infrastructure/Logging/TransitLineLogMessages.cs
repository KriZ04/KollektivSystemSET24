using Microsoft.Extensions.Logging;
using KollektivSystem.ApiService.Services;

namespace KollektivSystem.ApiService.Infrastructure.Logging
{
    internal static partial class TransitLineLogMessages
    {
        [LoggerMessage(
            Level = LogLevel.Information,
            Message = "Transit line {lineId} ({lineName}) created successfully.")]
        public static partial void LogTransitLineCreated(this ILogger logger, int lineId, string lineName);

        [LoggerMessage(
            Level = LogLevel.Error,
            Message = "Error creating transit line {lineId} ({lineName}): {errorMessage}")]
        public static partial void LogTransitLineCreationFailed(this ILogger logger, int lineId, string lineName, string errorMessage);

        [LoggerMessage(
            Level = LogLevel.Warning,
            Message = "Transit line with ID {lineId} not found.")]
        public static partial void LogTransitLineNotFound(this ILogger logger, int lineId);

        [LoggerMessage(
            Level = LogLevel.Information,
            Message = "Transit line {lineId} ({lineName}) updated successfully.")]
        public static partial void LogTransitLineUpdated(this ILogger logger, int lineId, string lineName);

        [LoggerMessage(
            Level = LogLevel.Error,
            Message = "Error updating transit line {lineId}: {errorMessage}")]
        public static partial void LogTransitLineUpdateFailed(this ILogger logger, int lineId, string errorMessage);

        [LoggerMessage(
            Level = LogLevel.Information,
            Message = "Transit line {lineId} ({lineName}) deleted successfully.")]
        public static partial void LogTransitLineDeleted(this ILogger logger, int lineId, string lineName);

        [LoggerMessage(
            Level = LogLevel.Error,
            Message = "Error deleting transit line {lineId}: {errorMessage}")]
        public static partial void LogTransitLineDeleteFailed(this ILogger logger, int lineId, string errorMessage);
    }
}
