namespace KollektivSystem.ApiService.Models.Transport
{
    public class Route
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Stop> Stops { get; set; } = new List<Stop>();
    }
}
