using Basket.API.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.API.Extensions
{
    public static class RedisConfiguration
    {
        public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionSetting =
                configuration.GetSection("RedisConnectionSettings").Get<RedisConnectionSetting>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionSetting.ConnectionString;
            });
        }
    }
}