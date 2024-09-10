using CatalogService.API.Configurations;
using CatalogService.API.Seeding;
using MongoDB.Driver;

namespace CatalogService.API.Extensions;
public static class MongoDbExtensions
{

    public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoSettings>(configuration.GetSection("MongoSettings"));
        services.AddSingleton<IMongoClient>(sp =>
            new MongoClient(configuration.GetValue<string>("MongoSettings:ConnectionString")));
    }

    public static async Task SeedDatabaseAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var seedService = scope.ServiceProvider.GetRequiredService<ICatalogSeedService>();
        await seedService.SeedAsync();
    }
}

