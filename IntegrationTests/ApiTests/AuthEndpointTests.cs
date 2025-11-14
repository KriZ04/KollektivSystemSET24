using KollektivSystem.ApiService;
using KollektivSystem.IntegrationTests.ApiTests.Setup;
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
        Assert.Fail();
    }
    [Fact]
    void Callback_WithInvalidState_ReturnsBadRequest()
    {
        Assert.Fail();
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
    void Refresh_WithInvalidToken_ReturnsUnauthorized()
    {
        Assert.Fail();
    }
    [Fact]
    void Refresh_WithValidToken_ReturnsOkAndNewTokens()
    {
        Assert.Fail();
    }
}
