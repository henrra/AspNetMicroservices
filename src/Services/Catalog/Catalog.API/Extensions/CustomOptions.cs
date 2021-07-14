using Catalog.API.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.API.Extensions
{
    public static class CustomOptions
    {
        public static void AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseSetting>(configuration.GetSection("DatabaseSettings"));
        }    
    }
}