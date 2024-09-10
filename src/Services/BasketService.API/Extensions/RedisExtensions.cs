using StackExchange.Redis;

namespace BasketService.API.Extensions;
public static class RedisExtensions
    {
        public static void AddRedisConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConfiguration = configuration.GetValue<string>("Redis:ConnectionString");
            var connectionMultiplexer = ConnectionMultiplexer.Connect(redisConfiguration);
            services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
        }
    }