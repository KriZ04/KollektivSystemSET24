namespace KollektivSystem.ApiService.Models.Dtos;

public sealed record UserMeDto(Guid Id, string Name, string? Email, string Role);
