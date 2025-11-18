namespace KollektivSystem.Web.Models;

public sealed class TicketTypeAdminDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public float Price { get; set; }
    public string AliveTime { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
