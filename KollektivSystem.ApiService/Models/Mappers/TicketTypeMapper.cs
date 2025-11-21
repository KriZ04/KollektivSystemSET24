using KollektivSystem.ApiService.Models.Dtos.Tickets;

namespace KollektivSystem.ApiService.Models.Mappers;

public static class TicketTypeMapper
{
    public static TicketTypeResponse ToResponse(this TicketType t) =>
        new()
        {
            Id = t.Id,
            Name = t.Name,
            Price = t.Price,
            AliveTime = t.AliveTime,
            IsActive = t.IsActive
        };
}
