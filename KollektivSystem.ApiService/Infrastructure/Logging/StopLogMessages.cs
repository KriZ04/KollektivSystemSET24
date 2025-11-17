using Microsoft.Extensions.Logging;
using KollektivSystem.ApiService.Services;

namespace KollektivSystem.ApiService.Infrastructure.Logging
{
    internal static partial class StopLogMessages
    {
        [LoggerMessage(
            EventId = 5101,
            Level = LogLevel.Information,
            Message = "Stop {stopId} ({stopName}) created successfully.")]
        internal static partial void LogStopCreated(this ILogger<StopService> logger, int stopId, string stopName);

        [LoggerMessage(
            EventId = 5102,
            Level = LogLevel.Warning,
            Message = "Stop with ID {stopId} not found.")]
        internal static partial void LogStopNotFound(this ILogger<StopService> logger, int stopId);

        [LoggerMessage(
            EventId = 5103,
            Level = LogLevel.Error,
            Message = "Error creating stop: {errorMessage}")]
        internal static partial void LogStopCreationFailed(this ILogger<StopService> logger, string errorMessage);

        [LoggerMessage(
            EventId = 5104,
            Level = LogLevel.Information,
            Message = "Stop {stopId} ({stopName}) updated successfully.")]
        internal static partial void LogStopUpdated(this ILogger<StopService> logger, int stopId, string stopName);

        [LoggerMessage(
            EventId = 5105,
            Level = LogLevel.Error,
            Message = "Error updating stop {stopId}: {errorMessage}")]
        internal static partial void LogStopUpdateFailed(this ILogger<StopService> logger, int stopId, string errorMessage);

        [LoggerMessage(
            EventId = 5106,
            Level = LogLevel.Information,
            Message = "Stop {stopId} deleted successfully.")]
        internal static partial void LogStopDeleted(this ILogger<StopService> logger, int stopId);
    }
}
