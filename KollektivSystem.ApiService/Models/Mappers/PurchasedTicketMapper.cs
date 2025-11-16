using KollektivSystem.ApiService.Models.Dtos.Tickets;

namespace KollektivSystem.ApiService.Models.Mappers;

public static class PurchasedTicketMapper
{
    public static PurchasedTicketResponse ToResponse(this PurchasedTicket t)
    {
        return new PurchasedTicketResponse
        {
            Id = t.Id,
            TicketTypeId = t.TicketTypeId,
            TicketTypeName = t.TicketType.Name,
            PurchasedAt = t.PurchasedAt,
            ExpireAt = t.ExpireAt,
            Revoked = t.Revoked,
            IsExpired = t.IsExpired,
            ValidationCode = t.ValidationCode
        };
    }
}
