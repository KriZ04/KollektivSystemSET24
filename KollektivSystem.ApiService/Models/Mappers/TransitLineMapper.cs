using KollektivSystem.ApiService.Models.Dtos.TransitLines;
using System.Runtime.CompilerServices;

namespace KollektivSystem.ApiService.Models.Mappers
{
    public static class TransitLineMapper
    {
        public static TransitLineResponse ToResponse(this TransitLine tl) =>
            new()
            {
                Id = tl.Id,
                Name = tl.Name
            };
    }
}
