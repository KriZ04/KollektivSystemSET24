namespace KollektivSystem.ApiService.Services
{
    public interface ITokenService
    {
        Task<(bool Success, string? AccessToken, string? RefreshToken)> RefreshAsync(string refreshToken, CancellationToken ct = default);
    }
}
