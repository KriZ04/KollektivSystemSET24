using KollektivSystem.ApiService.Extensions.Endpoints;
using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Domain;
using KollektivSystem.ApiService.Models.Enums;
using KollektivSystem.ApiService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KollektivSystem.UnitTests.ApiTests;

public class AuthEndpointsTests
{
    [Fact]
    public void Login_MissingReturnUrl_ReturnsBadRequest()
    {
        // Arrange
        ILogger<AuthEndpointsLoggerCategory> logger = NullLogger<AuthEndpointsLoggerCategory>.Instance;
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost");

        var request = context.Request;

        var authProvider = new Mock<IAuthProvider>(MockBehavior.Strict);
        var cache = new Mock<IMemoryCache>(MockBehavior.Strict);

        // Act
        var result = AuthEndpoints.HandleLogin(request, authProvider.Object, cache.Object, logger);

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
        ILogger<AuthEndpointsLoggerCategory> logger = NullLogger<AuthEndpointsLoggerCategory>.Instance;
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost");

        context.Request.QueryString = new QueryString("?returnUrl=not-valid-url");

        var request = context.Request;

        var authProvider = new Mock<IAuthProvider>(MockBehavior.Strict);
        var cache = new Mock<IMemoryCache>(MockBehavior.Strict);

        // Act
        var result = AuthEndpoints.HandleLogin(request, authProvider.Object, cache.Object, logger);

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
        ILogger<AuthEndpointsLoggerCategory> logger = NullLogger<AuthEndpointsLoggerCategory>.Instance;
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost");

        context.Request.QueryString = new QueryString("?returnUrl=ftp://example.com");

        var request = context.Request;

        var authProvider = new Mock<IAuthProvider>(MockBehavior.Strict);
        var cache = new Mock<IMemoryCache>(MockBehavior.Strict);


        // Act
        var result = AuthEndpoints.HandleLogin(request, authProvider.Object, cache.Object, logger);

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
        ILogger<AuthEndpointsLoggerCategory> logger = NullLogger<AuthEndpointsLoggerCategory>.Instance;
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
        var result = AuthEndpoints.HandleLogin(request, authProvider.Object, cache, logger);

        // Assert

        var redirect = Assert.IsType<RedirectHttpResult>(result);
        Assert.Equal(authUrl.ToString(), redirect.Url);


    }
    [Fact]
    public void Login_ValidReturnUrl_StoresStateInCache()
    {
        // Arrange
        ILogger<AuthEndpointsLoggerCategory> logger = NullLogger<AuthEndpointsLoggerCategory>.Instance;
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
        var result = AuthEndpoints.HandleLogin(request, authProvider.Object, cache, logger);

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
        ILogger<AuthEndpointsLoggerCategory> logger = NullLogger<AuthEndpointsLoggerCategory>.Instance;
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
        var result = AuthEndpoints.HandleLogin(request, authProvider.Object, cache, logger);

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
        ILogger<AuthEndpointsLoggerCategory> logger = NullLogger<AuthEndpointsLoggerCategory>.Instance;
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost");

        var request = context.Request;

        var cache = new MemoryCache(new MemoryCacheOptions());
        var provider = new Mock<IAuthProvider>(MockBehavior.Strict);
        var auth = new Mock<IAuthService>(MockBehavior.Strict);

        string? code = null;
        string? state = "some-state";

        var result = await AuthEndpoints.HandleCallback(code, state, request, cache, provider.Object, auth.Object, logger, CancellationToken.None);

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
        ILogger<AuthEndpointsLoggerCategory> logger = NullLogger<AuthEndpointsLoggerCategory>.Instance;
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost");

        var request = context.Request;

        var cache = new MemoryCache(new MemoryCacheOptions());
        var provider = new Mock<IAuthProvider>(MockBehavior.Strict);
        var auth = new Mock<IAuthService>(MockBehavior.Strict);

        string? code = "some-code";
        string? state = null;

        var result = await AuthEndpoints.HandleCallback(code, state, request, cache, provider.Object, auth.Object, logger, CancellationToken.None);

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
        ILogger<AuthEndpointsLoggerCategory> logger = NullLogger<AuthEndpointsLoggerCategory>.Instance;
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost");

        var request = context.Request;

        var cache = new MemoryCache(new MemoryCacheOptions());
        var provider = new Mock<IAuthProvider>(MockBehavior.Strict);
        var auth = new Mock<IAuthService>(MockBehavior.Strict);

        string? code = "some-code";
        string? state = "some-state";

        var result = await AuthEndpoints.HandleCallback(code, state, request, cache, provider.Object, auth.Object, logger, CancellationToken.None);

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
        ILogger<AuthEndpointsLoggerCategory> logger = NullLogger<AuthEndpointsLoggerCategory>.Instance;
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

        var result = await AuthEndpoints.HandleCallback(code, state, request, cache, provider.Object, auth.Object, logger, CancellationToken.None);

        var statusResult = Assert.IsType<IStatusCodeHttpResult>(result, exactMatch: false);
        Assert.Equal(StatusCodes.Status400BadRequest, statusResult.StatusCode);


        var valueResult = Assert.IsType<IValueHttpResult>(result, exactMatch: false);
        Assert.NotNull(valueResult.Value);

        Assert.True(cache.TryGetValue(cacheKey, out var cachedValue));
        Assert.True(string.IsNullOrWhiteSpace(cachedValue as string));

        provider.VerifyNoOtherCalls();
        auth.VerifyNoOtherCalls();
    }
    [Fact]
    public async Task Callback_InvalidReturnUrlInCache_ReturnsBadRequest()
    {
        ILogger<AuthEndpointsLoggerCategory> logger = NullLogger<AuthEndpointsLoggerCategory>.Instance;
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
        var invalidReturnUrl = "not-valid-url";
        cache.Set(cacheKey, invalidReturnUrl);

        var result = await AuthEndpoints.HandleCallback(code, state, request, cache, provider.Object, auth.Object, logger, CancellationToken.None);

        var statusResult = Assert.IsType<IStatusCodeHttpResult>(result, exactMatch: false);
        Assert.Equal(StatusCodes.Status400BadRequest, statusResult.StatusCode);


        var valueResult = Assert.IsType<IValueHttpResult>(result, exactMatch: false);
        Assert.NotNull(valueResult.Value);

        Assert.True(cache.TryGetValue(cacheKey, out var cachedValue));
        Assert.Equal(invalidReturnUrl, cachedValue);

        provider.VerifyNoOtherCalls();
        auth.VerifyNoOtherCalls();
    }
    [Fact]
    public async Task Callback_ValidRequest_ExchangesCodeViaAuthProvider()
    {
        // Arrange
        ILogger<AuthEndpointsLoggerCategory> logger = NullLogger<AuthEndpointsLoggerCategory>.Instance;
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost");

        var request = context.Request;

        var cache = new MemoryCache(new MemoryCacheOptions());

        string? code = "valid-auth-code-123";
        string? state = "valid-state-abc";

        var cacheKey = $"oidc_state:{state}";
        var returnUrl = "https://localhost:7151/home";
        cache.Set(cacheKey, returnUrl);

        var provider = new Mock<IAuthProvider>();

        var redurectUriExpected = new Uri("https://localhost/auth/callback");

        var tokens = new TokenResult("id-token-123", "access.token", "refresh-token");

        provider.Setup(p => p.ExchangeCodeAsync(
            It.IsAny<string>(),
            It.IsAny<Uri>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(tokens);

        var fakeUser = new User { Id = Guid.Parse("A9AFB356-2ED5-461D-838C-87208F45D589"), DisplayName = "User Name", Sub = "sub-user", Role = Role.Customer };

        var principal = new ClaimsPrincipal(
            new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, fakeUser.Id.ToString()),
                new Claim(ClaimTypes.Role, fakeUser.Role.ToString())
            }, "oidc"));


        provider.Setup(p => p.ValidateAndReadIdToken(tokens.IdToken)).Returns(principal);

        var auth = new Mock<IAuthService>();

        var apiJwt = "api-jwt-abc";
        var refreshToken = "refresh-xyz";
        var providerType = AuthProvider.Mock;

        provider.SetupGet(p => p.Provider).Returns(providerType);

        auth
            .Setup(a => a.SignInWithIdTokenAsync(
                providerType,
                principal,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((fakeUser, apiJwt, refreshToken));

        // Act
        var result = await AuthEndpoints.HandleCallback(code, state, request, cache, provider.Object, auth.Object, logger, CancellationToken.None);

        // Assert
        provider.Verify(p => p.ExchangeCodeAsync(
                code,
                It.Is<Uri>(u =>
                    u.Scheme == "https" &&
                    u.Host == "localhost" &&
                    u.AbsolutePath.EndsWith("/auth/callback")),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    [Fact]
    public async Task Callback_ValidRequest_ValidatesIdTokenAndSignsInUser()
    {
        // Arrange
        ILogger<AuthEndpointsLoggerCategory> logger = NullLogger<AuthEndpointsLoggerCategory>.Instance;
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost");

        var request = context.Request;

        var cache = new MemoryCache(new MemoryCacheOptions());

        string? code = "valid-auth-code-123";
        string? state = "valid-state-abc";

        var cacheKey = $"oidc_state:{state}";
        var returnUrl = "https://localhost:7151/home";
        cache.Set(cacheKey, returnUrl);

        var provider = new Mock<IAuthProvider>();

        var redurectUriExpected = new Uri("https://localhost/auth/callback");

        var tokens = new TokenResult("id-token-123", "access.token", "refresh-token");

        provider.Setup(p => p.ExchangeCodeAsync(
            It.IsAny<string>(),
            It.IsAny<Uri>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(tokens);

        var fakeUser = new User { Id = Guid.Parse("A9AFB356-2ED5-461D-838C-87208F45D589"), DisplayName = "User Name", Sub = "sub-user", Role = Role.Customer };

        var principal = new ClaimsPrincipal(
            new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, fakeUser.Id.ToString()),
                new Claim(ClaimTypes.Role, fakeUser.Role.ToString())
            }, "oidc"));


        provider.Setup(p => p.ValidateAndReadIdToken(tokens.IdToken)).Returns(principal);

        var auth = new Mock<IAuthService>();

        var apiJwt = "api-jwt-abc";
        var refreshToken = "refresh-xyz";
        var providerType = AuthProvider.Mock;

        provider.SetupGet(p => p.Provider).Returns(providerType);

        auth
            .Setup(a => a.SignInWithIdTokenAsync(
                providerType,
                principal,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((fakeUser, apiJwt, refreshToken));

        // Act
        var result = await AuthEndpoints.HandleCallback(code, state, request, cache, provider.Object, auth.Object, logger, CancellationToken.None);

        // Assert
        provider.Verify(p => p.ValidateAndReadIdToken(tokens.IdToken), Times.Once);
        
        auth.Verify(a => a.SignInWithIdTokenAsync(
        providerType,
        principal,
        It.IsAny<CancellationToken>()),
    Times.Once);
    }
    [Fact]
    public async Task Callback_ValidRequest_CreatesRedirectWithTokenAndRefreshToken()
    {
        // Arrange
        ILogger<AuthEndpointsLoggerCategory> logger = NullLogger<AuthEndpointsLoggerCategory>.Instance;
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost");

        var request = context.Request;

        var cache = new MemoryCache(new MemoryCacheOptions());

        string? code = "valid-auth-code-123";
        string? state = "valid-state-abc";

        var cacheKey = $"oidc_state:{state}";
        var returnUrl = "https://localhost:7151/home";
        cache.Set(cacheKey, returnUrl);

        var provider = new Mock<IAuthProvider>();

        var redurectUriExpected = new Uri("https://localhost/auth/callback");

        var tokens = new TokenResult("id-token-123", "access.token", "refresh-token");

        provider.Setup(p => p.ExchangeCodeAsync(
            It.IsAny<string>(),
            It.IsAny<Uri>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(tokens);

        var fakeUser = new User { Id = Guid.Parse("A9AFB356-2ED5-461D-838C-87208F45D589"), DisplayName = "User Name", Sub = "sub-user", Role = Role.Customer };

        var principal = new ClaimsPrincipal(
            new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, fakeUser.Id.ToString()),
                new Claim(ClaimTypes.Role, fakeUser.Role.ToString())
            }, "oidc"));


        provider.Setup(p => p.ValidateAndReadIdToken(tokens.IdToken)).Returns(principal);

        var auth = new Mock<IAuthService>();

        var apiJwt = "api-jwt-abc";
        var refreshToken = "refresh-xyz";
        var providerType = AuthProvider.Mock;

        provider.SetupGet(p => p.Provider).Returns(providerType);

        auth
            .Setup(a => a.SignInWithIdTokenAsync(
                providerType,
                principal,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((fakeUser, apiJwt, refreshToken));

        // Act
        var result = await AuthEndpoints.HandleCallback(code, state, request, cache, provider.Object, auth.Object, logger, CancellationToken.None);

        // Assert
        var redirect = Assert.IsType<RedirectHttpResult>(result);

        var url = redirect.Url;
        Assert.NotNull(url);

        Assert.Contains(apiJwt, url);
        Assert.Contains(refreshToken, url);

        Assert.StartsWith(returnUrl, url);
    }
    [Fact]
    public async Task Callback_ValidRequest_RemovesStateFromCache()
    {
        // Arrange
        ILogger<AuthEndpointsLoggerCategory> logger = NullLogger<AuthEndpointsLoggerCategory>.Instance;
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost");

        var request = context.Request;

        var cache = new MemoryCache(new MemoryCacheOptions());

        string? code = "valid-auth-code-123";
        string? state = "valid-state-abc";

        var cacheKey = $"oidc_state:{state}";
        var returnUrl = "https://localhost:7151/home";
        cache.Set(cacheKey, returnUrl);

        var provider = new Mock<IAuthProvider>();

        var redurectUriExpected = new Uri("https://localhost/auth/callback");

        var tokens = new TokenResult("id-token-123", "access.token", "refresh-token");

        provider.Setup(p => p.ExchangeCodeAsync(
            It.IsAny<string>(),
            It.IsAny<Uri>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(tokens);

        var fakeUser = new User { Id = Guid.Parse("A9AFB356-2ED5-461D-838C-87208F45D589"), DisplayName = "User Name", Sub = "sub-user", Role = Role.Customer };

        var principal = new ClaimsPrincipal(
            new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, fakeUser.Id.ToString()),
                new Claim(ClaimTypes.Role, fakeUser.Role.ToString())
            }, "oidc"));


        provider.Setup(p => p.ValidateAndReadIdToken(tokens.IdToken)).Returns(principal);

        var auth = new Mock<IAuthService>();

        var apiJwt = "api-jwt-abc";
        var refreshToken = "refresh-xyz";
        var providerType = AuthProvider.Mock;

        provider.SetupGet(p => p.Provider).Returns(providerType);

        auth
            .Setup(a => a.SignInWithIdTokenAsync(
                providerType,
                principal,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((fakeUser, apiJwt, refreshToken));

        // Act
        var result = await AuthEndpoints.HandleCallback(code, state, request, cache, provider.Object, auth.Object, logger, CancellationToken.None);

        // Assert
        Assert.False(cache.TryGetValue(cacheKey, out _));


    }
    [Fact]
    public async Task Callback_ValidRequest_ReturnsTemporaryRedirect()
    {
        // Arrange
        ILogger<AuthEndpointsLoggerCategory> logger = NullLogger<AuthEndpointsLoggerCategory>.Instance;
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost");

        var request = context.Request;

        var cache = new MemoryCache(new MemoryCacheOptions());

        string? code = "valid-auth-code-123";
        string? state = "valid-state-abc";

        var cacheKey = $"oidc_state:{state}";
        var returnUrl = "https://localhost:7151/home";
        cache.Set(cacheKey, returnUrl);

        var provider = new Mock<IAuthProvider>();

        var redurectUriExpected = new Uri("https://localhost/auth/callback");

        var tokens = new TokenResult("id-token-123", "access.token", "refresh-token");

        provider.Setup(p => p.ExchangeCodeAsync(
            It.IsAny<string>(),
            It.IsAny<Uri>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(tokens);

        var fakeUser = new User { Id = Guid.Parse("A9AFB356-2ED5-461D-838C-87208F45D589"), DisplayName = "User Name", Sub = "sub-user", Role = Role.Customer };

        var principal = new ClaimsPrincipal(
            new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, fakeUser.Id.ToString()),
                new Claim(ClaimTypes.Role, fakeUser.Role.ToString())
            }, "oidc"));


        provider.Setup(p => p.ValidateAndReadIdToken(tokens.IdToken)).Returns(principal);

        var auth = new Mock<IAuthService>();

        var apiJwt = "api-jwt-abc";
        var refreshToken = "refresh-xyz";
        var providerType = AuthProvider.Mock;

        provider.SetupGet(p => p.Provider).Returns(providerType);

        auth
            .Setup(a => a.SignInWithIdTokenAsync(
                providerType,
                principal,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((fakeUser, apiJwt, refreshToken));

        // Act
        var result = await AuthEndpoints.HandleCallback(code, state, request, cache, provider.Object, auth.Object, logger, CancellationToken.None);

        // Assert
        var redirect = Assert.IsType<RedirectHttpResult>(result);
        Assert.NotNull(redirect.Url);
        Assert.False(redirect.Permanent);
    }
    [Fact]
    public async Task Refresh_RefreshFails_ReturnsUnauthorized()
    {
        // Arrange
        ILogger<AuthEndpointsLoggerCategory> logger = NullLogger<AuthEndpointsLoggerCategory>.Instance;
        var req = new AuthEndpoints.RefreshRequest("bad-refresh-token");

        var tokenService = new Mock<ITokenService>();

        bool isSuccess = false;
        string? accessToken = null;
        string? refreshToken = null;

        tokenService
            .Setup(s => s.RefreshAsync(
                req.RefreshToken,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((isSuccess, accessToken, refreshToken));
        // Act
        var result = await AuthEndpoints.HandleRefresh(req, tokenService.Object, logger, CancellationToken.None);

        // Assert
        var statusResult = Assert.IsType<IStatusCodeHttpResult>(result, exactMatch: false);
        Assert.Equal(StatusCodes.Status401Unauthorized, statusResult.StatusCode);

        Assert.IsType<UnauthorizedHttpResult>(result);

        tokenService.Verify(s => s.RefreshAsync(
            req.RefreshToken,
            It.IsAny<CancellationToken>()),
            Times.Once);

        tokenService.VerifyNoOtherCalls();
    }
    [Fact]
    public async Task Refresh_ValidRefreshToken_ReturnsOk()
    {
        // Arrange
        ILogger<AuthEndpointsLoggerCategory> logger = NullLogger<AuthEndpointsLoggerCategory>.Instance;
        var req = new AuthEndpoints.RefreshRequest("valid-refresh-token");

        var tokenService = new Mock<ITokenService>();

        bool isSuccess = true;
        string? accessToken = "access.token.abc";
        string? refreshToken = "valid-refresh-token";

        tokenService
            .Setup(s => s.RefreshAsync(
                req.RefreshToken,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((isSuccess, accessToken, refreshToken));
        // Act
        var result = await AuthEndpoints.HandleRefresh(req, tokenService.Object, logger, CancellationToken.None);

        // Assert
        var statusResult = Assert.IsType<IStatusCodeHttpResult>(result, exactMatch: false);
        Assert.Equal(StatusCodes.Status200OK, statusResult.StatusCode);


    }
    [Fact]
    public async Task Refresh_ValidRefreshToken_ReturnsNewAccessAndRefreshTokens()
    {
        // Arrange
        ILogger<AuthEndpointsLoggerCategory> logger = NullLogger<AuthEndpointsLoggerCategory>.Instance;
        var req = new AuthEndpoints.RefreshRequest("valid-refresh-token");

        var tokenService = new Mock<ITokenService>();

        bool isSuccess = true;
        string? accessToken = "access.token.abc";
        string? refreshToken = "valid-refresh-token";

        tokenService
            .Setup(s => s.RefreshAsync(
                req.RefreshToken,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((isSuccess, accessToken, refreshToken));
        // Act
        var result = await AuthEndpoints.HandleRefresh(req, tokenService.Object, logger, CancellationToken.None);

        // Assert
        var valueResult = Assert.IsType<IValueHttpResult>(result, exactMatch: false);
        Assert.NotNull(valueResult.Value);

        dynamic body = valueResult.Value;

        Assert.Equal(accessToken, (string)body.access_token);
        Assert.Equal(refreshToken, (string)body.refresh_token);
    }
    [Fact]
    public async Task Refresh_ValidRefreshToken_CallsTokenServiceOnce()
    {
        // Arrange
        ILogger<AuthEndpointsLoggerCategory> logger = NullLogger<AuthEndpointsLoggerCategory>.Instance;
        var req = new AuthEndpoints.RefreshRequest("valid-refresh-token");

        var tokenService = new Mock<ITokenService>();

        bool isSuccess = true;
        string? accessToken = "access.token.abc";
        string? refreshToken = "valid-refresh-token";

        tokenService
            .Setup(s => s.RefreshAsync(
                req.RefreshToken,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((isSuccess, accessToken, refreshToken));
        // Act
        var result = await AuthEndpoints.HandleRefresh(req, tokenService.Object, logger, CancellationToken.None);

        // Assert
        tokenService.Verify(s => s.RefreshAsync(
            req.RefreshToken,
            It.IsAny<CancellationToken>()),
        Times.Once);

        tokenService.VerifyNoOtherCalls();
    }


}
