namespace KollektivSystem.Web.Models
{
    public sealed record AdminUserDto(
        Guid Id,
        string? Name,
        string? Email,
        string Role,
        string? Provider,
        string? Sub,
        DateTime? LastLogin
    );
}
