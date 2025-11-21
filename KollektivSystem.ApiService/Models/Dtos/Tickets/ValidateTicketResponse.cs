namespace KollektivSystem.ApiService.Models.Dtos.Tickets;

public sealed class ValidateTicketResponse
{
    public bool IsValid { get; init; }
    public string? Reason { get; init; }

    public Guid? TicketId { get; init; }
    public DateTimeOffset? ExpireAt { get; init; }
}
