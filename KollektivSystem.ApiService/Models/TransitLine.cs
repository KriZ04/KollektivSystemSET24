namespace KollektivSystem.ApiService.Models
{
    public class TransitLine
    {
        public int Id { get; init; }
        public string Name { get; set; } = string.Empty;
        public ICollection<TransitLineStop> Stops { get; set; } = new List<TransitLineStop>();
    }
}
