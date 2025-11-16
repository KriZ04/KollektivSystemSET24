namespace KollektivSystem.ApiService.Models
{
    public class Tickets
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
