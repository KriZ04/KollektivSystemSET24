namespace KollektivSystem.ApiService.Models.Dtos.Tickets;

public class PurchasedTicketResponse
{
    public Guid Id { get; init; }

    public int TicketTypeId { get; init; }
    public string TicketTypeName { get; init; } = null!;

    public DateTimeOffset PurchasedAt { get; init; }
    public DateTimeOffset ExpireAt { get; init; }

    public bool Revoked { get; init; }
    public bool IsExpired { get; init; }

    public string ValidationCode { get; init; } = null!;
}
