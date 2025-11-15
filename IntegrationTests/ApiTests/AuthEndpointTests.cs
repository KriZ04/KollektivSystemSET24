using KollektivSystem.ApiService;
using KollektivSystem.IntegrationTests.ApiTests.Setup;
using KollektivSystem.IntegrationTests.ApiTests.TestClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KollektivSystem.IntegrationTests.ApiTests;

public class AuthEndpointTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory _factory;

    public AuthEndpointTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        _factory = factory;
    }

    [Fact]
    public async Task Login_WithMissingReturnUrl_ReturnsBadRequest()
    {
        // Arrange
        var url = $"/auth/login";

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    [Fact]
    public async Task Login_WithInvalidReturnUrl_ReturnsBadRequest()
    {
        // Arrange
        var invalidReturnUrl = "invalid-return-url";
        var url = $"/auth/login?returnUrl={Uri.EscapeDataString(invalidReturnUrl)}";

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithValidReturnUrl_ReturnsRedirectToAuthorizeEndpoint()
    {
        // Arrange
        var returnUrl = "https://localhost:7151/home";
        var url = $"/auth/login?returnUrl={Uri.EscapeDataString(returnUrl)}";

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

        // Redirect to oidc
        var location = response.Headers.Location;
        Assert.NotNull(location);

        Assert.NotEqual(returnUrl, location!.ToString());
    }
    [Fact]
    public async Task Callback_WithMissingParameters_ReturnsBadRequest()
    {
        // Arrange
        var state = "valid-state-123";
        var returnUrl = "https://localhost:7151/home";

        using (var scope = _factory.Services.CreateScope())
        {
            var cache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
            cache.Set($"oidc_state:{state}", returnUrl);
        }


        var url = $"/auth/callback";

        // Act
        var response = await _client.GetAsync(url);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);


    }
    [Fact]
    public async Task Callback_WithInvalidState_ReturnsBadRequest()
    {
        // Arrange
        var state = "invalid-state";
        var code = "valid-auth-code-abc";

        var url = $"/auth/callback?code={Uri.EscapeDataString(code)}&state={Uri.EscapeDataString(state)}";

        // Act
        var response = await _client.GetAsync(url);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

    }
    [Fact]
    public async Task Callback_WithValidStateAndValidCode_RedirectsWithTokensAttached()
    {
        // Arrange
        var state = "valid-state-123";
        var returnUrl = "https://localhost:7151/home";

        using (var scope = _factory.Services.CreateScope())
        {
            var cache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
            cache.Set($"oidc_state:{state}", returnUrl);
        }

        var code = "valid-auth-code-abc";

        var url = $"/auth/callback?code={Uri.EscapeDataString(code)}&state={Uri.EscapeDataString(state)}";

        // Act
        var response = await _client.GetAsync(url);

        //Assert
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

        var location = response.Headers.Location;
        Assert.NotNull(location);

        var redirectUrl = location!.ToString();

        Assert.StartsWith(returnUrl, redirectUrl);


        var uri = new Uri(redirectUrl);
        var query = QueryHelpers.ParseQuery(uri.Query);

        Assert.True(query.ContainsKey("token"));
        Assert.True(query.ContainsKey("refresh"));

        Assert.False(string.IsNullOrWhiteSpace(query["token"]));
        Assert.False(string.IsNullOrWhiteSpace(query["refresh"]));


    }

    [Fact]
    public async Task Refresh_WithInvalidToken_ReturnsUnauthorized()
    { 
        // Arrange
        var payload = new
        {
            refreshToken = "some-invalid-refresh-token"
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/auth/refresh", content);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    [Fact]
    public async Task Refresh_WithValidToken_ReturnsOkAndNewTokens()
    {
        // Arrange
        var payload = new
        {
            refreshToken = TestTokenService.ValidTestRefreshToken
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/auth/refresh", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(body));

        using var doc = JsonDocument.Parse(body);
        var root = doc.RootElement;

        Assert.True(root.TryGetProperty("access_token", out var accessProp));
        Assert.True(root.TryGetProperty("refresh_token", out var refreshProp));

        var accessToken = accessProp.GetString();
        var newRefreshToken = refreshProp.GetString();

        Assert.False(string.IsNullOrWhiteSpace(accessToken));
        Assert.False(string.IsNullOrWhiteSpace(newRefreshToken));
    }
}
