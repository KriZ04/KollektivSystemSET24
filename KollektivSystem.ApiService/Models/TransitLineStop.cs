namespace KollektivSystem.ApiService.Models
{
    public class TransitLineStop
    {
        public int Id { get; set; }

        public int Order { get; set; }

        public int TransitLineId { get; set; }
        public TransitLine TransitLine { get; set; } = null!;

        public int StopId { get; set; }
        public Stop Stop { get; set; } = null!;
    }
}