using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Repositories.Uow;
using KollektivSystem.ApiService.Services.Implementations;
using KollektivSystem.ApiService.Services.Interfaces;

namespace KollektivSystem.ApiService.Extensions.ServiceExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            //Unit of Work
            services.AddScoped<IUnitOfWork, ApplicationUnitOfWork>();

            // Repositories
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
    }

}
