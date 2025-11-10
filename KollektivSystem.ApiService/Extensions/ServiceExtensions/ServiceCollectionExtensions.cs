using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models.Enums;
using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Repositories.Uow;
using KollektivSystem.ApiService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace KollektivSystem.ApiService.Extensions.ServiceExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, ApplicationUnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Example for domain-specific repos
            //services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public static IServiceCollection AddAuths(this IServiceCollection services, IConfiguration config)
        {
            var apiJwtKey = config["Jwt:SigningKey"] ?? "super-secret-and-long-secret-key-that-is-at-least-32-bytes";
            var apiIssuer = config["Jwt:Issuer"] ?? "app";
            var apiAudience = config["Jwt:Audience"] ?? "app-clients";

            var oidcIssuer = config["Oidc:Issuer"] ?? "https://localhost";
            var oidcClientId = config["Oidc:ClientId"] ?? "demo-client";
            var oidcClientSecret = config["Oidc:ClientSecret"] ?? "demo-secret";
            var oidcSigningKey = config["Oidc:SigningKey"] ?? "super-secret-and-long-secret-key-that-is-at-least-32-bytes";

            services.AddMemoryCache();

            

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = apiIssuer,
                        ValidAudience = apiAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiJwtKey)),
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true
                    };
                });

            
            services.AddAuthorization(o =>
            {
                o.AddPolicy("Admin", p => p.RequireRole(nameof(Role.Admin)));
                o.AddPolicy("Developer", p => p.RequireRole(nameof(Role.Developer)));
                o.AddPolicy("Staff", p => p.RequireRole(nameof(Role.Admin), nameof(Role.Developer)));
                o.AddPolicy("RegisteredUser", p => p.RequireRole(nameof(Role.Customer), nameof(Role.Admin), nameof(Role.Developer)));
            });

            services.AddHttpClient("oidc");

            services.AddSingleton<IJwtIssuer>(new JwtIssuer(apiIssuer, apiAudience, apiJwtKey));
            services.AddSingleton<IAuthProvider>(sp =>
            {
                var http = sp.GetRequiredService<IHttpClientFactory>().CreateClient("oidc");
                return new MockAuthProvider(oidcIssuer, oidcClientId, oidcClientSecret, oidcSigningKey, http);
            });
            services.AddScoped<IAuthService, AuthService>();

            return services;

        }
    }
}
