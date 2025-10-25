using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OidcStub.Extensions
{
    public static class OidcExtensions
    {
        public static IServiceCollection AddOidcStub(this IServiceCollection services, IConfiguration config)
        {
            services.AddMemoryCache();
            services.Configure<>();
            return services;
        }
    }
}
