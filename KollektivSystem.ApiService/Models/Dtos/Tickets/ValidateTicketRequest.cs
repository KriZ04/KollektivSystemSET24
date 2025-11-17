namespace KollektivSystem.ApiService.Models.Dtos.Tickets;

public sealed class ValidateTicketRequest
{
    public string ValidationCode { get; init; } = null!;
}
