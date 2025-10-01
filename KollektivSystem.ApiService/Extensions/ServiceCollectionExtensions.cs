using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Repositories.Uow;

namespace KollektivSystem.ApiService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, ApplicationUnitOfWork>();
            services.AddScoped(typeof(IRepository<,>), typeof(EfRepository<,>));

            // Example for domain-specific repos
            //services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
