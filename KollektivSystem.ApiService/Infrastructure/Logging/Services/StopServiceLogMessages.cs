using Microsoft.Extensions.Logging;
using KollektivSystem.ApiService.Services;

namespace KollektivSystem.ApiService.Infrastructure.Logging
{
    internal static partial class StopServiceLogMessages
    {
        private const int Base = LogAreas.Stop;

        [LoggerMessage(
            EventId = Base + 100,
            Level = LogLevel.Information,
            Message = "Stop {stopId} ({stopName}) created successfully.")]
        internal static partial void LogStopCreated(this ILogger logger, int stopId, string stopName);

        [LoggerMessage(
            EventId = Base + 101,
            Level = LogLevel.Warning,
            Message = "Stop with ID {stopId} not found.")]
        internal static partial void LogStopNotFound(this ILogger logger, int stopId);

        [LoggerMessage(
            EventId = Base + 102,
            Level = LogLevel.Error,
            Message = "Error creating stop: {errorMessage}")]
        internal static partial void LogStopCreationFailed(this ILogger logger, string errorMessage);

        [LoggerMessage(
            EventId = Base + 103,
            Level = LogLevel.Information,
            Message = "Stop {stopId} ({stopName}) updated successfully.")]
        internal static partial void LogStopUpdated(this ILogger logger, int stopId, string stopName);

        [LoggerMessage(
            EventId = Base + 104,
            Level = LogLevel.Error,
            Message = "Error updating stop {stopId}: {errorMessage}")]
        internal static partial void LogStopUpdateFailed(this ILogger logger, int stopId, string errorMessage);

        [LoggerMessage(
            EventId = Base + 105,
            Level = LogLevel.Information,
            Message = "Stop {stopId} deleted successfully.")]
        internal static partial void LogStopDeleted(this ILogger logger, int stopId);
    }
}
