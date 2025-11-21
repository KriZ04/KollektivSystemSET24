using Microsoft.Extensions.Logging;

namespace KollektivSystem.ApiService.Infrastructure.Logging
{
    internal static partial class TransitLineStopServiceLogMessages
    {
        private const int Base = LogAreas.TransitLineStop;
        // CREATE
        [LoggerMessage(
            EventId = Base + 100,
            Level = LogLevel.Warning,
            Message = "Cannot create TransitLineStop: Line {lineId} already has a stop at order {order}.")]
        public static partial void LogOrderAlreadyExists(this ILogger logger, int lineId, int order);

        [LoggerMessage(
            EventId = Base + 101,
            Level = LogLevel.Warning,
            Message = "Cannot create TransitLineStop: Line {lineId} already contains Stop {stopId}.")]
        public static partial void LogStopAlreadyExists(this ILogger logger, int lineId, int stopId);

        [LoggerMessage(
            EventId = Base + 102,
            Level = LogLevel.Information,
            Message = "TransitLineStop created successfully: Id {id}, TransitLineId {lineId}, StopId {stopId}, Order {order}.")]
        public static partial void LogTransitLineStopCreated(this ILogger logger, int id, int lineId, int stopId, int order);

        [LoggerMessage(
            EventId = Base + 103,
            Level = LogLevel.Error,
            Message = "Error creating TransitLineStop for TransitLineId {lineId}, StopId {stopId}, Order {order}: {errorMessage}")]
        public static partial void LogTransitLineStopCreationFailed(this ILogger logger, int lineId, int stopId, int order, string errorMessage);


        // NOT FOUND
        [LoggerMessage(
            EventId = Base + 105,
            Level = LogLevel.Warning,
            Message = "TransitLineStop with ID {id} not found.")]
        public static partial void LogTransitLineStopNotFound(this ILogger logger, int id);

        [LoggerMessage(
            EventId = Base + 106,
            Level = LogLevel.Warning,
            Message = "TransitLineStop with ID {id} not found for update.")]
        public static partial void LogTransitLineStopNotFoundForUpdate(this ILogger logger, int id);

        [LoggerMessage(
            EventId = Base + 107,
            Level = LogLevel.Warning,
            Message = "TransitLineStop with ID {id} not found for deletion.")]
        public static partial void LogTransitLineStopNotFoundForDeletion(this ILogger logger, int id);


        // UPDATE
        [LoggerMessage(
            EventId = Base + 110,
            Level = LogLevel.Information,
            Message = "TransitLineStop {id} updated successfully.")]
        public static partial void LogTransitLineStopUpdated(this ILogger logger, int id);

        [LoggerMessage(
            EventId = Base + 111,
            Level = LogLevel.Error,
            Message = "Error updating TransitLineStop {id}: {errorMessage}")]
        public static partial void LogTransitLineStopUpdateFailed(this ILogger logger, int id, string errorMessage);


        // DELETE
        [LoggerMessage(
            EventId = Base + 115,
            Level = LogLevel.Information,
            Message = "TransitLineStop {id} deleted successfully.")]
        public static partial void LogTransitLineStopDeleted(this ILogger logger, int id);

        [LoggerMessage(
            EventId = Base + 116,
            Level = LogLevel.Error,
            Message = "Error deleting TransitLineStop {id}: {errorMessage}")]
        public static partial void LogTransitLineStopDeleteFailed(this ILogger logger, int id, string errorMessage);
    }
}
