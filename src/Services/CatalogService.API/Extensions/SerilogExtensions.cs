namespace CatalogService.API.Extensions;
public static class SerilogExtensions
{
    public static void AddSerilog(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        services.AddSingleton(Log.Logger);
    }
}