namespace KollektivSystem.ApiService.Models.Domain
{
    public record AuthChallenge(Uri RedirectUri, string State, string? CodeVerifier);
    public record TokenResult(string IdToken, string? AccessToken, string? RefreshToken);
}
