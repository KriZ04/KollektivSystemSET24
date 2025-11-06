using KollektivSystem.ApiService.Infrastructure;
using KollektivSystem.ApiService.Models.Enums;
using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Repositories.Uow;
using KollektivSystem.ApiService.Services.Implementations;
using KollektivSystem.ApiService.Services.Interfaces;
using KollektivSystem.ApiService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace KollektivSystem.ApiService.Extensions.ServiceExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, ApplicationUnitOfWork>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITransitLineRepository, TransitLineRepository>();
            services.AddScoped<ITicketsRepository, TicketsRepository>();


            return services;
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            // Domain Services
            services.AddScoped<ITransitLineService, TransitLineService>();

            return services;
        }

        public static IServiceCollection AddAuths(this IServiceCollection services, IConfiguration config)
        {
            var apiJwtKey = config["Jwt:SigningKey"] ?? "dev-api-hmac-key-change";
            var apiIssuer = config["Jwt:Issuer"] ?? "app";
            var apiAudience = config["Jwt:Audience"] ?? "app-clients";

            var oidcIssuer = config["Oidc:Issuer"] ?? "https://idp.local";
            var oidcClientId = config["Oidc:ClientId"] ?? "demo-client";
            var oidcClientSecret = config["Oidc:ClientSecret"] ?? "demo-secret";
            var oidcSigningKey = config["Oidc:SigningKey"] ?? "dev-idp-hmac-key";

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
