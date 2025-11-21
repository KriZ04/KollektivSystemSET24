namespace KollektivSystem.Web.Models;

public sealed class PurchasedTicketDto
{
    public Guid Id { get; set; }

    public int TicketTypeId { get; set; }
    public string TicketTypeName { get; set; } = string.Empty;

    public DateTimeOffset PurchasedAt { get; set; }
    public DateTimeOffset ExpireAt { get; set; }

    public bool Revoked { get; set; }
    public bool IsExpired { get; set; }

    public string ValidationCode { get; set; } = string.Empty;
}
