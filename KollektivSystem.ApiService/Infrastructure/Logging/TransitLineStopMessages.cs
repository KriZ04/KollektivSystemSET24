using Microsoft.Extensions.Logging;

namespace KollektivSystem.ApiService.Infrastructure.Logging
{
    internal static partial class TransitLineStopLogMessages
    {
        // CREATE
        [LoggerMessage(
            Level = LogLevel.Warning,
            Message = "Cannot create TransitLineStop: Line {lineId} already has a stop at order {order}")]
        public static partial void LogOrderAlreadyExists(this ILogger logger, int lineId, int order);

        [LoggerMessage(
            Level = LogLevel.Warning,
            Message = "Cannot create TransitLineStop: Line {lineId} already contains Stop {stopId}")]
        public static partial void LogStopAlreadyExists(this ILogger logger, int lineId, int stopId);

        [LoggerMessage(
            Level = LogLevel.Information,
            Message = "Created TransitLineStop: Id {id}, TransitLineId {lineId}, StopId {stopId}, Order {order}")]
        public static partial void LogTransitLineStopCreated(this ILogger logger, int id, int lineId, int stopId, int order);


        // NOT FOUND
        [LoggerMessage(
            Level = LogLevel.Warning,
            Message = "TransitLineStop with ID {id} not found.")]
        public static partial void LogTransitLineStopNotFound(this ILogger logger, int id);

        [LoggerMessage(
            Level = LogLevel.Warning,
            Message = "TransitLineStop with ID {id} not found for update.")]
        public static partial void LogTransitLineStopNotFoundForUpdate(this ILogger logger, int id);

        [LoggerMessage(
            Level = LogLevel.Warning,
            Message = "TransitLineStop with ID {id} not found for deletion.")]
        public static partial void LogTransitLineStopNotFoundForDeletion(this ILogger logger, int id);


        // UPDATE
        [LoggerMessage(
            Level = LogLevel.Information,
            Message = "Updated TransitLineStop with ID {id}.")]
        public static partial void LogTransitLineStopUpdated(this ILogger logger, int id);


        // DELETE
        [LoggerMessage(
            Level = LogLevel.Information,
            Message = "Deleted TransitLineStop with ID {id}.")]
        public static partial void LogTransitLineStopDeleted(this ILogger logger, int id);
    }
}
