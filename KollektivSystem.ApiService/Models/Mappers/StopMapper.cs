using KollektivSystem.ApiService.Models.Dtos.Stops;

namespace KollektivSystem.ApiService.Models.Mappers;

public static class StopMapper
{
    public static StopResponse ToResponse(this Stop s) =>
        new()
        {
            Id = s.Id,
            Name = s.Name,
            Latitude = s.Latitude,
            Longitude = s.Longitude,
        };
}
