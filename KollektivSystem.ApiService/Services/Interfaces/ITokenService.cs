namespace KollektivSystem.ApiService.Services.Interfaces
{
    public interface ITokenService
    {
        Task<(bool Success, string? AccessToken, string? RefreshToken)> RefreshAsync(string refreshToken, CancellationToken ct = default);
    }
}
