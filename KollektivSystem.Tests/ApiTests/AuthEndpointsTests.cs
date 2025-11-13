using KollektivSystem.ApiService.Extensions.Endpoints;
using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models.Domain;
using KollektivSystem.ApiService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KollektivSystem.UnitTests.ApiTests;

public class AuthEndpointsTests
{
    [Fact]
    public void Login_MissingReturnUrl_ReturnsBadRequest()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost");

        var request = context.Request;

        var authProvider = new Mock<IAuthProvider>(MockBehavior.Strict);
        var cache = new Mock<IMemoryCache>(MockBehavior.Strict);

        // Act
        var result = AuthEndpoints.HandleLogin(request, authProvider.Object, cache.Object);

        // Assert
        var statusResult = Assert.IsType<IStatusCodeHttpResult>(result, exactMatch: false);
        Assert.Equal(StatusCodes.Status400BadRequest, statusResult.StatusCode);

        var valueResult = Assert.IsType<IValueHttpResult>(result, exactMatch: false);
        Assert.NotNull(valueResult.Value);

        authProvider.VerifyNoOtherCalls();
        cache.VerifyNoOtherCalls();


    }
    [Fact]
    public void Login_InvalidReturnUrl_ReturnsBadRequest()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost");

        context.Request.QueryString = new QueryString("?returnUrl=not-valid-url");

        var request = context.Request;

        var authProvider = new Mock<IAuthProvider>(MockBehavior.Strict);
        var cache = new Mock<IMemoryCache>(MockBehavior.Strict);

        // Act
        var result = AuthEndpoints.HandleLogin(request, authProvider.Object, cache.Object);

        // Assert
        var statusResult = Assert.IsType<IStatusCodeHttpResult>(result, exactMatch: false);
        Assert.Equal(StatusCodes.Status400BadRequest, statusResult.StatusCode);

        var valueResult = Assert.IsType<IValueHttpResult>(result, exactMatch: false);
        Assert.NotNull(valueResult.Value);

        authProvider.VerifyNoOtherCalls();
        cache.VerifyNoOtherCalls();
    }
    [Fact]
    public void Login_ReturnUrlWithUnsupportedScheme_ReturnsBadRequest()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost");

        context.Request.QueryString = new QueryString("?returnUrl=ftp://example.com");

        var request = context.Request;

        var authProvider = new Mock<IAuthProvider>(MockBehavior.Strict);
        var cache = new Mock<IMemoryCache>(MockBehavior.Strict);


        // Act
        var result = AuthEndpoints.HandleLogin(request, authProvider.Object, cache.Object);

        // Assert
        var statusResult = Assert.IsType<IStatusCodeHttpResult>(result, exactMatch: false);
        Assert.Equal(StatusCodes.Status400BadRequest, statusResult.StatusCode);

        var valueResult = Assert.IsType<IValueHttpResult>(result, exactMatch: false);
        Assert.NotNull(valueResult.Value);

        authProvider.VerifyNoOtherCalls();
        cache.VerifyNoOtherCalls();
    }
    [Fact]
    public void Login_ValidReturnUrl_ReturnsRedirectResult()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost");

        var returnUrl = "https://localhost:7151/home";
        context.Request.QueryString = new QueryString($"?returnUrl={Uri.EscapeDataString(returnUrl)}");

        var request = context.Request;

        var authProvider = new Mock<IAuthProvider>();

        var state = "abc123";
        var authUrl = new Uri($"htttps://oidc.local/authorize?state={state}");

        var challenge = new AuthChallenge(authUrl, state, null);

        authProvider.Setup(p => p.BuildAuthorizeRedirect(
            It.IsAny<Uri>(),
            It.IsAny<string[]>()))
            .Returns(challenge);


        var cache = new MemoryCache(new MemoryCacheOptions());

        // Act
        var result = AuthEndpoints.HandleLogin(request, authProvider.Object, cache);

        // Assert

        var redirect = Assert.IsType<RedirectHttpResult>(result);
        Assert.Equal(authUrl.ToString(), redirect.Url);


    }
    [Fact]
    public void Login_ValidReturnUrl_StoresStateInCache()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost");

        var returnUrl = "https://localhost:7151/home";
        context.Request.QueryString = new QueryString($"?returnUrl={Uri.EscapeDataString(returnUrl)}");

        var request = context.Request;

        var authProvider = new Mock<IAuthProvider>();

        var state = "abc123";
        var authUrl = new Uri($"htttps://oidc.local/authorize?state={state}");

        var challenge = new AuthChallenge(authUrl, state, null);

        authProvider.Setup(p => p.BuildAuthorizeRedirect(
            It.IsAny<Uri>(),
            It.IsAny<string[]>()))
            .Returns(challenge);


        var cache = new MemoryCache(new MemoryCacheOptions());

        // Act
        var result = AuthEndpoints.HandleLogin(request, authProvider.Object, cache);

        // Assert

        var cacheKey = $"oidc_state:{state}";
        Assert.True(cache.TryGetValue(cacheKey, out var cachedValue));
        Assert.Equal(returnUrl, cachedValue);

        Assert.IsType<RedirectHttpResult>(result);

    }
    [Fact]
    public void Login_ValidReturnUrl_UsesAuthProviderToBuildAuthorizeUrl()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost");

        var returnUrl = "https://localhost:7151/home";
        context.Request.QueryString = new QueryString($"?returnUrl={Uri.EscapeDataString(returnUrl)}");

        var request = context.Request;

        var authProvider = new Mock<IAuthProvider>();

        var state = "abc123";
        var authUrl = new Uri($"htttps://oidc.local/authorize?state={state}");

        var challenge = new AuthChallenge(authUrl, state, null);

        authProvider.Setup(p => p.BuildAuthorizeRedirect(
            It.IsAny<Uri>(),
            It.IsAny<string[]>()))
            .Returns(challenge);


        var cache = new MemoryCache(new MemoryCacheOptions());

        // Act
        var result = AuthEndpoints.HandleLogin(request, authProvider.Object, cache);

        // Assert
        authProvider.Verify(p => p.BuildAuthorizeRedirect(
            It.Is<Uri>(u =>
                u.Scheme == "https" &&
                u.Host == "localhost" &&
                u.AbsolutePath.EndsWith("/auth/callback")),
            It.Is<string[]>(scopes =>
                scopes.Contains("openid") &&
                scopes.Contains("email") &&
                scopes.Contains("profile"))),
            Times.Once);

        authProvider.VerifyNoOtherCalls();

        Assert.NotNull(result);
    }
    [Fact]
    public async Task Callback_MissingCode_ReturnsBadRequest()
    {
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost");

        var request = context.Request;

        var cache = new MemoryCache(new MemoryCacheOptions());
        var provider = new Mock<IAuthProvider>(MockBehavior.Strict);
        var auth = new Mock<IAuthService>(MockBehavior.Strict);

        string? code = null;
        string? state = "some-state";

        var result = await AuthEndpoints.HandleCallback(code, state, request, cache, provider.Object, auth.Object, CancellationToken.None);

        var statusResult = Assert.IsType<IStatusCodeHttpResult>(result, exactMatch: false);
        Assert.Equal(StatusCodes.Status400BadRequest, statusResult.StatusCode);


        var valueResult = Assert.IsType<IValueHttpResult>(result, exactMatch: false);
        Assert.NotNull(valueResult.Value);

        provider.VerifyNoOtherCalls();
        auth.VerifyNoOtherCalls();

    }
    [Fact]
    public async Task Callback_MissingState_ReturnsBadRequest()
    {
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost");

        var request = context.Request;

        var cache = new MemoryCache(new MemoryCacheOptions());
        var provider = new Mock<IAuthProvider>(MockBehavior.Strict);
        var auth = new Mock<IAuthService>(MockBehavior.Strict);

        string? code = "some-code";
        string? state = null;

        var result = await AuthEndpoints.HandleCallback(code, state, request, cache, provider.Object, auth.Object, CancellationToken.None);

        var statusResult = Assert.IsType<IStatusCodeHttpResult>(result, exactMatch: false);
        Assert.Equal(StatusCodes.Status400BadRequest, statusResult.StatusCode);


        var valueResult = Assert.IsType<IValueHttpResult>(result, exactMatch: false);
        Assert.NotNull(valueResult.Value);

        provider.VerifyNoOtherCalls();
        auth.VerifyNoOtherCalls();

    }
    [Fact]
    public async Task Callback_StateNotInCache_ReturnsBadRequest()
    {
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost");

        var request = context.Request;

        var cache = new MemoryCache(new MemoryCacheOptions());
        var provider = new Mock<IAuthProvider>(MockBehavior.Strict);
        var auth = new Mock<IAuthService>(MockBehavior.Strict);

        string? code = "some-code";
        string? state = "some-state";

        var result = await AuthEndpoints.HandleCallback(code, state, request, cache, provider.Object, auth.Object, CancellationToken.None);

        var statusResult = Assert.IsType<IStatusCodeHttpResult>(result, exactMatch: false);
        Assert.Equal(StatusCodes.Status400BadRequest, statusResult.StatusCode);


        var valueResult = Assert.IsType<IValueHttpResult>(result, exactMatch: false);
        Assert.NotNull(valueResult.Value);

        provider.VerifyNoOtherCalls();
        auth.VerifyNoOtherCalls();

    }
    [Fact]
    public async Task Callback_EmptyReturnUrlInCache_ReturnsBadRequest()
    {
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost");

        var request = context.Request;

        var cache = new MemoryCache(new MemoryCacheOptions());
        var provider = new Mock<IAuthProvider>(MockBehavior.Strict);
        var auth = new Mock<IAuthService>(MockBehavior.Strict);

        string? code = "some-code";
        string? state = "some-state";

        var cacheKey = $"oidc_state:{state}";
        cache.Set(cacheKey, "");

        var result = await AuthEndpoints.HandleCallback(code, state, request, cache, provider.Object, auth.Object, CancellationToken.None);

        var statusResult = Assert.IsType<IStatusCodeHttpResult>(result, exactMatch: false);
        Assert.Equal(StatusCodes.Status400BadRequest, statusResult.StatusCode);


        var valueResult = Assert.IsType<IValueHttpResult>(result, exactMatch: false);
        Assert.NotNull(valueResult.Value);

        Assert.True(cache.TryGetValue(cacheKey, out var cachedValue));
        Assert.True(string.IsNullOrWhiteSpace(cachedValue as string));

        provider.VerifyNoOtherCalls();
        auth.VerifyNoOtherCalls();
    }
    //[Fact]
    //void Callback_InvalidReturnUrlInCache_ReturnsBadRequest()
    //{
    //    Assert.Fail();
    //}
    //[Fact]
    //void Callback_ValidRequest_ExchangesCodeViaAuthProvider()
    //{
    //    Assert.Fail();
    //}
    //[Fact]
    //void Callback_ValidRequest_ValidatesIdTokenAndSignsInUser()
    //{
    //    Assert.Fail();
    //}
    //[Fact]
    //void Callback_ValidRequest_CreatesRedirectWithTokenAndRefreshToken()
    //{
    //    Assert.Fail();
    //}
    //[Fact]
    //void Callback_ValidRequest_RemovesStateFromCache()
    //{
    //    Assert.Fail();
    //}
    //[Fact]
    //void Callback_ValidRequest_ReturnsTemporaryRedirect()
    //{
    //    Assert.Fail();
    //}
    //[Fact]
    //void Refresh_InvalidRefreshToken_ReturnsUnauthorized()
    //{
    //    Assert.Fail();
    //}
    //[Fact]
    //void Refresh_RefreshFails_ReturnsUnauthorized()
    //{
    //    Assert.Fail();
    //}
    //[Fact]
    //void Refresh_ValidRefreshToken_ReturnsOk()
    //{
    //    Assert.Fail();
    //}
    //[Fact]
    //void Refresh_ValidRefreshToken_ReturnsNewAccessAndRefreshTokens()
    //{
    //    Assert.Fail();
    //}
    //[Fact]
    //void Refresh_ValidRefreshToken_CallsTokenServiceOnce()
    //{
    //    Assert.Fail();
    //}


}
