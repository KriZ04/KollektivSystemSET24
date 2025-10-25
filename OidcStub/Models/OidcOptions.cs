namespace OidcStub.Models
{
    public sealed class OidcOptions
    {
        public List<PersonaDto> Personas { get; init; } = new();
        public Dictionary<string, string> ClientPersonas { get; init; } = new();
    }
}
