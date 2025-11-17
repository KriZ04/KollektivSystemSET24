using KollektivSystem.ApiService.Infrastructure.Logging;

namespace KollektivSystem.ApiService.Extensions.Endpoints;

public static partial class PurchasedTicketEndpointsLogMessages
{
    private const int Base = LogAreas.PurchasedTicket;

    // Buy
    [LoggerMessage(EventId = Base + 0, Level = LogLevel.Information,
        Message = "Buy ticket requested by user {userId} for TicketType {ticketTypeId}.")]
    public static partial void BuyTicketRequested(this ILogger logger, Guid userId, int ticketTypeId);

    [LoggerMessage(EventId = Base + 1, Level = LogLevel.Error,
        Message = "Buy ticket failed for user {userId} and TicketType {ticketTypeId}.")]
    public static partial void BuyTicketFailed(this ILogger logger, Guid userId, int ticketTypeId);

    [LoggerMessage(EventId = Base + 2, Level = LogLevel.Information,
        Message = "Ticket {ticketId} purchased by user {userId} for TicketType {ticketTypeId}.")]
    public static partial void BuyTicketSucceeded(this ILogger logger, Guid ticketId, Guid userId, int ticketTypeId);


    // Validate
    [LoggerMessage(EventId = Base + 10, Level = LogLevel.Information,
        Message = "Validation requested for ticket code '{validationCode}'.")]
    public static partial void TicketValidationRequested(this ILogger logger, string validationCode);

    [LoggerMessage(EventId = Base + 11, Level = LogLevel.Warning,
        Message = "Ticket validation failed for code '{validationCode}': {reason}.")]
    public static partial void TicketValidationFailed(this ILogger logger, string validationCode, string reason);

    [LoggerMessage(EventId = Base + 12, Level = LogLevel.Information,
        Message = "Ticket {ticketId} validated successfully for code '{validationCode}'.")]
    public static partial void TicketValidationSucceeded(this ILogger logger, Guid ticketId, string validationCode);


    // GetMe / GetMeById
    [LoggerMessage(EventId = Base + 20, Level = LogLevel.Information,
        Message = "Listing tickets for user {userId}. IncludeInvalid={includeInvalid}.")]
    public static partial void GetMyTicketsRequested(this ILogger logger, Guid userId, bool includeInvalid);

    [LoggerMessage(EventId = Base + 21, Level = LogLevel.Information,
        Message = "Ticket {ticketId} fetched for user {userId}.")]
    public static partial void GetMyTicketByIdSucceeded(this ILogger logger, Guid userId, Guid ticketId);

    [LoggerMessage(EventId = Base + 22, Level = LogLevel.Warning,
        Message = "Ticket {ticketId} not found for user {userId}.")]
    public static partial void GetMyTicketByIdNotFound(this ILogger logger, Guid userId, Guid ticketId);


    // Revoke
    [LoggerMessage(EventId = Base + 30, Level = LogLevel.Information,
        Message = "Revoke requested for ticket {ticketId}.")]
    public static partial void RevokeTicketRequested(this ILogger logger, Guid ticketId);

    [LoggerMessage(EventId = Base + 31, Level = LogLevel.Information,
        Message = "Ticket {ticketId} revoked successfully.")]
    public static partial void RevokeTicketSucceeded(this ILogger logger, Guid ticketId);

    [LoggerMessage(EventId = Base + 32, Level = LogLevel.Warning,
        Message = "Revoke failed: ticket {ticketId} not found.")]
    public static partial void RevokeTicketNotFound(this ILogger logger, Guid ticketId);


    // GetById (admin / global)
    [LoggerMessage(EventId = Base + 40, Level = LogLevel.Information, Message = "Ticket {ticketId} fetch requested.")]
    public static partial void GetTicketByIdRequested(this ILogger logger, Guid ticketId);
    [LoggerMessage(EventId = Base + 41, Level = LogLevel.Information,
        Message = "Ticket {ticketId} fetched.")]
    public static partial void GetTicketByIdSucceeded(this ILogger logger, Guid ticketId);

    [LoggerMessage(EventId = Base + 42, Level = LogLevel.Warning,
        Message = "Ticket {ticketId} not found.")]
    public static partial void GetTicketByIdNotFound(this ILogger logger, Guid ticketId);
}
