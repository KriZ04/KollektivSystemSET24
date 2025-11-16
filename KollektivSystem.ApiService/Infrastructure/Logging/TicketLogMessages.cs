using Microsoft.Extensions.Logging;
using KollektivSystem.ApiService.Services;

namespace KollektivSystem.ApiService.Infrastructure.Logging
{
    internal static partial class TicketLogMessages
    {
        // CREATE
        [LoggerMessage(
            Level = LogLevel.Information,
            Message = "Ticket {ticketId} created successfully.")]
        internal static partial void LogTicketCreated(this ILogger logger, int ticketId);

        [LoggerMessage(
            Level = LogLevel.Error,
            Message = "Error creating ticket: {errorMessage}")]
        internal static partial void LogTicketCreationFailed(this ILogger logger, string errorMessage);

        // NOT FOUND
        [LoggerMessage(
            Level = LogLevel.Warning,
            Message = "Ticket {ticketId} not found.")]
        internal static partial void LogTicketNotFound(this ILogger logger, int ticketId);

        // DELETE
        [LoggerMessage(
            Level = LogLevel.Information,
            Message = "Ticket {ticketId} deleted successfully.")]
        internal static partial void LogTicketDeleted(this ILogger logger, int ticketId);

        // UPDATE
        [LoggerMessage(
            Level = LogLevel.Information,
            Message = "Ticket with ID {ticketId} updated successfully.")]
        internal static partial void LogTicketUpdated(this ILogger logger, int ticketId);

        [LoggerMessage(
            Level = LogLevel.Error,
            Message = "Failed to update ticket: {errorMessage}")]
        internal static partial void LogTicketUpdateFailed(this ILogger logger, string errorMessage);
    }
}
