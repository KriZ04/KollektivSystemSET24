namespace KollektivSystem.Web.Models;

public sealed class TransitLineStopDto
{
    public int Id { get; set; }
    public int Order { get; set; }
    public int TransitLineId { get; set; }
    public int StopId { get; set; }
}
