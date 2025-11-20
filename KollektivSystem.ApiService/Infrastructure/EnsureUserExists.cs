using KollektivSystem.ApiService.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using KollektivSystem.ApiService.Infrastructure.Logging;
using KollektivSystem.ApiService.Infrastructure.Logging.Infrastructure;

namespace KollektivSystem.ApiService.Infrastructure;

public sealed class EnsureUserExists
{
    private readonly RequestDelegate _next;
    private readonly ILogger<EnsureUserExists> _logger;

    public EnsureUserExists(RequestDelegate next, ILogger<EnsureUserExists> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IUserRepository users)
    {
        var endpoint = context.GetEndpoint();

        if (endpoint?.Metadata.GetMetadata<IAuthorizeData>() is null)
        {
            await _next(context);
            return;
        }

        var principal = context.User;

        if (principal?.Identity?.IsAuthenticated != true)
        {
            await _next(context);
            return;
        }

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            _logger.InvalidOrMissingNameIdClaim();
            await WriteUnauthorized(context);
            return;
        }

        var exists = await users.ExistsAsync(userId, context.RequestAborted);

        if (!exists)
        {
            _logger.UserDoesNotExist(userId);

            await WriteUnauthorized(context);
            return;
        }

        await _next(context);
    }

    private static async Task WriteUnauthorized(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Unauthorized");
    }
}
