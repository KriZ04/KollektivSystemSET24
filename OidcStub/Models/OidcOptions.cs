namespace OidcStub.Models
{
    public sealed class OidcOptions
    {
        public List<PersonaDto> Personas { get; init; } = new();
        public Dictionary<string, string> ClientPersonas { get; init; } = new();

        public string ClientId { get; init; } = default!;
        public string ClientSecret { get; init; } = default!;
        public string Issuer { get; init; } = default!;
        public string SigningKey { get; init; } = default!;

    }
}
