namespace KollektivSystem.ApiService.Infrastructure.Logging.Endpoints;

public static partial class TicketTypeEndpointsLogMessages
{
    private const int Base = LogAreas.TicketType;
    // List (all vs active)
    [LoggerMessage(EventId = Base + 0, Level = LogLevel.Information,
        Message = "Listing ticket types. ActiveOnly={activeOnly}.")]
    public static partial void ListTicketTypesRequested(this ILogger logger, bool activeOnly);

    [LoggerMessage(EventId = Base + 1, Level = LogLevel.Information,
        Message = "Returned {count} ticket types. ActiveOnly={activeOnly}.")]
    public static partial void ListTicketTypesSucceeded(this ILogger logger, int count, bool activeOnly);

    [LoggerMessage(EventId = Base + 2, Level = LogLevel.Warning,
        Message = "No ticket types found. ActiveOnly={activeOnly}.")]
    public static partial void ListTicketTypesNotFound(this ILogger logger, bool activeOnly);


    // Get by id (all vs active)
    [LoggerMessage(EventId = Base + 10, Level = LogLevel.Information,
        Message = "Fetching ticket type {ticketTypeId}. ActiveOnly={activeOnly}.")]
    public static partial void GetTicketTypeByIdRequested(this ILogger logger, int ticketTypeId, bool activeOnly);

    [LoggerMessage(EventId = Base + 11, Level = LogLevel.Warning,
        Message = "Ticket type {ticketTypeId} not found. ActiveOnly={activeOnly}.")]
    public static partial void GetTicketTypeByIdNotFound(this ILogger logger, int ticketTypeId, bool activeOnly);

    [LoggerMessage(EventId = Base + 12, Level = LogLevel.Information,
        Message = "Ticket type {ticketTypeId} fetched. ActiveOnly={activeOnly}.")]
    public static partial void GetTicketTypeByIdSucceeded(this ILogger logger, int ticketTypeId, bool activeOnly);


    // Create
    [LoggerMessage(EventId = Base + 20, Level = LogLevel.Information,
        Message = "Create ticket type requested.")]
    public static partial void CreateTicketTypeRequested(this ILogger logger);

    [LoggerMessage(EventId = Base + 21, Level = LogLevel.Error,
        Message = "Create ticket type failed.")]
    public static partial void CreateTicketTypeFailed(this ILogger logger);

    [LoggerMessage(EventId = Base + 22, Level = LogLevel.Information,
        Message = "Ticket type {ticketTypeId} created.")]
    public static partial void CreateTicketTypeSucceeded(this ILogger logger, int ticketTypeId);


    // Status change (Activate / Deactivate)
    [LoggerMessage(EventId = Base + 30, Level = LogLevel.Information,
        Message = "{action} ticket type {ticketTypeId} requested.")]
    public static partial void TicketTypeStatusChangeRequested(this ILogger logger, int ticketTypeId, string action);

    [LoggerMessage(EventId = Base + 31, Level = LogLevel.Warning,
        Message = "{action} failed: ticket type {ticketTypeId} not found.")]
    public static partial void TicketTypeStatusChangeNotFound(this ILogger logger, int ticketTypeId, string action);

    [LoggerMessage(EventId = Base + 32, Level = LogLevel.Information,
        Message = "Ticket type {ticketTypeId} {action} successfully.")]
    public static partial void TicketTypeStatusChangeSucceeded(this ILogger logger, int ticketTypeId, string action);
}

