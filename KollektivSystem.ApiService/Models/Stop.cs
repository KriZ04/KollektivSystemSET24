namespace KollektivSystem.ApiService.Models;

public class Stop
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
    public int RouteId { get; set; }
    public TransitLine? Route { get; set; }
}
