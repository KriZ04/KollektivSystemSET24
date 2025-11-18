namespace KollektivSystem.Web.Models;

public sealed class TicketDto
{
    public int Id { get; set; }

   
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }
}
