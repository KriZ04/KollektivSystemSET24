namespace KollektivSystem.Web.Models;

public sealed class PurchasedTicketDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime PurchasedAtUtc { get; set; }
    public DateTime ValidUntilUtc { get; set; }
    public string Code { get; set; } = string.Empty;
}
