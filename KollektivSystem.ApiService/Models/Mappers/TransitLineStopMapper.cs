using KollektivSystem.ApiService.Models.Dtos.TransitLineStops;

namespace KollektivSystem.ApiService.Models.Mappers;

public static class TransitLineStopMapper
{
    public static TransitLineStopResponse ToResponse(this TransitLineStop tls) =>
        new()
        {
            Id = tls.Id,
            TransitLineId = tls.TransitLineId,
            StopId = tls.StopId,
            Order = tls.Order
        };
}
