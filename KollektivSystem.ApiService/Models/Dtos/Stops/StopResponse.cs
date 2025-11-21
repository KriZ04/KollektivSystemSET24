namespace KollektivSystem.ApiService.Models.Dtos.Stops;

public sealed class StopResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public double Latitude { get; init; }
    public double Longitude { get; init; }
}
