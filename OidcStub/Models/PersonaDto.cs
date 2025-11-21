namespace OidcStub.Models
{
    public record PersonaDto(
        string Key,
        string Sub, 
        string Name,
        string? Email
    );
}
