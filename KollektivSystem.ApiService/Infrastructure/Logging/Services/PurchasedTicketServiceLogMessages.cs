using KollektivSystem.ApiService.Infrastructure.Logging;

namespace KollektivSystem.ApiService.Services;

public static partial class PurchasedTicketServiceLogMessages
{
    private const int Base = LogAreas.PurchasedTicket + 100;
    // GetByIdAsync
    [LoggerMessage(EventId = Base + 0, Level = LogLevel.Debug,
        Message = "Fetching ticket {ticketId}.")]
    public static partial void FetchTicket(this ILogger logger, Guid ticketId);

    [LoggerMessage(EventId = Base + 1, Level = LogLevel.Debug,
        Message = "Ticket {ticketId} not found.")]
    public static partial void TicketNotFound(this ILogger logger, Guid ticketId);


    // GetByUserAsync
    [LoggerMessage(EventId = Base + 10, Level = LogLevel.Debug,
        Message = "Fetching tickets for user {userId}. IncludeInvalid={includeInvalid}.")]
    public static partial void FetchUserTickets(this ILogger logger, Guid userId, bool includeInvalid);


    // PurchaseAsync
    [LoggerMessage(EventId = Base + 20, Level = LogLevel.Information,
        Message = "User {userId} is purchasing ticket type {ticketTypeId}.")]
    public static partial void PurchaseRequested(this ILogger logger, Guid userId, int ticketTypeId);

    [LoggerMessage(EventId = Base + 21, Level = LogLevel.Warning,
        Message = "Ticket type {ticketTypeId} not found for purchase.")]
    public static partial void TicketTypeNotFoundForPurchase(this ILogger logger, int ticketTypeId);

    [LoggerMessage(EventId = Base + 22, Level = LogLevel.Debug,
        Message = "Generated validation code {code}.")]
    public static partial void GeneratedValidationCode(this ILogger logger, string code);

    [LoggerMessage(EventId = Base + 23, Level = LogLevel.Information,
        Message = "User {userId} purchased ticket {ticketId}.")]
    public static partial void PurchaseSucceeded(this ILogger logger, Guid ticketId, Guid userId);


    // RevokeAsync
    [LoggerMessage(EventId = Base + 30, Level = LogLevel.Information,
        Message = "Revoking ticket {ticketId}.")]
    public static partial void RevokeRequested(this ILogger logger, Guid ticketId);

    [LoggerMessage(EventId = Base + 31, Level = LogLevel.Warning,
        Message = "Cannot revoke ticket {ticketId}: not found.")]
    public static partial void RevokeNotFound(this ILogger logger, Guid ticketId);

    [LoggerMessage(EventId = Base + 32, Level = LogLevel.Information,
        Message = "Ticket {ticketId} revoked successfully.")]
    public static partial void RevokeSucceeded(this ILogger logger, Guid ticketId);


    // ValidateAsync
    [LoggerMessage(EventId = Base + 40, Level = LogLevel.Debug,
        Message = "Validating ticket with code '{validationCode}'.")]
    public static partial void ValidationRequested(this ILogger logger, string validationCode);

    [LoggerMessage(EventId = Base + 41, Level = LogLevel.Warning,
        Message = "Ticket validation failed for code '{validationCode}': {reason}.")]
    public static partial void ValidationFailed(this ILogger logger, string validationCode, string reason);

    [LoggerMessage(EventId = Base + 42, Level = LogLevel.Information,
        Message = "Ticket {ticketId} validated successfully.")]
    public static partial void ValidationSucceeded(this ILogger logger, Guid ticketId);
}
