namespace KollektivSystem.ApiService.Models;

public class Stop
{
    public int Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public ICollection<TransitLineStop> TransitLineStops { get; set; } = new List<TransitLineStop>();
}
