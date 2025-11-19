using KollektivSystem.ApiService.Models.Dtos.Stops;

namespace KollektivSystem.ApiService.Models.Mappers;

public static class StopMapper
{
    public static StopMapper ToResponse(this Stop t) =>
        new()
        {
            Id = t.Id,
            Name = t.Name,
            Latitude = t.Latitude,
            Longitude = t.Longitude,
        };
}
