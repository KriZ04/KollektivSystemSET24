using KollektivSystem.ApiService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KollektivSystem.IntegrationTests.ApiTests.TestClasses
{
    internal class TestTokenService : ITokenService
    {
        public const string ValidTestRefreshToken = "test-valid-refresh-token";
        public Task<(bool Success, string? AccessToken, string? RefreshToken)> RefreshAsync(string refreshToken, CancellationToken ct = default)
        {
            if (refreshToken == ValidTestRefreshToken)
            {
                return Task.FromResult<(bool, string?, string?)>(
                    (true, "new-access-token-123", "new-refresh-token-456"));
            }

            return Task.FromResult<(bool, string?, string?)>(
                (false, null!, null!));
        }
    }
}
