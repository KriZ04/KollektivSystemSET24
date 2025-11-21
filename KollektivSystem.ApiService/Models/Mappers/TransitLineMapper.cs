using KollektivSystem.ApiService.Models.Dtos.TransitLines;

namespace KollektivSystem.ApiService.Models.Mappers;

public static class TransitLineMapper
{
    public static TransitLineResponse ToResponse(this TransitLine tl) =>
        new()
        {
            Id = tl.Id,
            Name = tl.Name
        };
}
