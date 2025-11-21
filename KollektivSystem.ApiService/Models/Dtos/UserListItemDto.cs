namespace KollektivSystem.ApiService.Models.Dtos;

public sealed record UserListItemDto(Guid Id, string Name, string? Email, string Role, string Provider, string Sub, DateTimeOffset? LastLogin, ICollection<PurchasedTicket> PurchasedTickets);
