using Microsoft.Extensions.Logging;
using KollektivSystem.ApiService.Services.Implementations;

namespace KollektivSystem.ApiService.Infrastructure.Logging
{
    public static partial class TicketLogMessages
    {
        [LoggerMessage(
            Level = LogLevel.Information,
            Message = "Ticket {ticketId} created successfully.")]
        public static partial void LogTicketCreated(this ILogger<TicketService> logger, int ticketId);

        [LoggerMessage(
            Level = LogLevel.Warning,
            Message = "Ticket {ticketId} not found.")]
        public static partial void LogTicketNotFound(this ILogger<TicketService> logger, int ticketId);

        [LoggerMessage(
            Level = LogLevel.Error,
            Message = "Error creating ticket: {errorMessage}")]
        public static partial void LogTicketCreationFailed(this ILogger<TicketService> logger, string errorMessage);

        [LoggerMessage(
            Level = LogLevel.Information,
            Message = "Ticket {ticketId} deleted successfully.")]
        public static partial void LogTicketDeleted(this ILogger<TicketService> logger, int ticketId);
    }
}
