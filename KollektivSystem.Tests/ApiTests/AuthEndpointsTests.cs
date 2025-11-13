using KollektivSystem.ApiService.Extensions.Endpoints;
using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace KollektivSystem.UnitTests.Api_tests
{
    public class AuthEndpointsTests
    {
        [Fact]
        void Login_MissingReturnUrl_ReturnsBadRequest()
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
            var statusResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, statusResult.StatusCode);

            var valueResult = Assert.IsAssignableFrom<IValueHttpResult>(result);
            Assert.NotNull(valueResult.Value);

            authProvider.VerifyNoOtherCalls();
            cache.VerifyNoOtherCalls();


        }
        [Fact]
        void Login_InvalidReturnUrl_ReturnsBadRequest()
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
            var statusResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, statusResult.StatusCode);

            var valueResult = Assert.IsAssignableFrom<IValueHttpResult>(result);
            Assert.NotNull(valueResult.Value);

            authProvider.VerifyNoOtherCalls();
            cache.VerifyNoOtherCalls();
        }
        [Fact]
        void Login_ReturnUrlWithUnsupportedScheme_ReturnsBadRequest()
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
            var statusResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, statusResult.StatusCode);

            var valueResult = Assert.IsAssignableFrom<IValueHttpResult>(result);
            Assert.NotNull(valueResult.Value);

            authProvider.VerifyNoOtherCalls();
            cache.VerifyNoOtherCalls();
        }
        [Fact]
        void Login_ValidReturnUrl_ReturnsRedirectResult()
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
        void Login_ValidReturnUrl_StoresStateInCache()
        {
            Assert.Fail();
        }
        [Fact]
        void Login_ValidReturnUrl_UsesAuthProviderToBuildAuthorizeUrl()
        {
            Assert.Fail();
        }
        [Fact]
        void Callback_MissingCodeOrState_ReturnsBadRequest()
        {
            Assert.Fail();
        }
        [Fact]
        void Callback_StateNotInCache_ReturnsBadRequest()
        {
            Assert.Fail();
        }
        [Fact]
        void Callback_EmptyReturnUrlInCache_ReturnsBadRequest()
        {
            Assert.Fail();
        }
        [Fact]
        void Callback_InvalidReturnUrl_ReturnsBadRequest()
        {
            Assert.Fail();
        }
        [Fact]
        void Callback_ValidRequest_ExchangesCodeViaAuthProvider()
        {
            Assert.Fail();
        }
        [Fact]
        void Callback_ValidRequest_ValidatesIdTokenAndSignsInUser()
        {
            Assert.Fail();
        }
        [Fact]
        void Callback_ValidRequest_CreatesRedirectWithTokenAndRefreshToken()
        {
            Assert.Fail();
        }
        [Fact]
        void Callback_ValidRequest_RemovesStateFromCache()
        {
            Assert.Fail();
        }
        [Fact]
        void Callback_ValidRequest_ReturnsTemporaryRedirect()
        {
            Assert.Fail();
        }
        [Fact]
        void Refresh_InvalidRefreshToken_ReturnsUnauthorized()
        {
            Assert.Fail();
        }
        [Fact]
        void Refresh_RefreshFails_ReturnsUnauthorized()
        {
            Assert.Fail();
        }
        [Fact]
        void Refresh_ValidRefreshToken_ReturnsOk()
        {
            Assert.Fail();
        }
        [Fact]
        void Refresh_ValidRefreshToken_ReturnsNewAccessAndRefreshTokens()
        {
            Assert.Fail();
        }
        [Fact]
        void Refresh_ValidRefreshToken_CallsTokenServiceOnce()
        {
            Assert.Fail();
        }


    }
}
