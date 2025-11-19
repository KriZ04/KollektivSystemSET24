namespace KollektivSystem.ApiService.Models.Dtos.TransitLines
{
    public sealed class CreateTransitLineRequest
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
    }
}
