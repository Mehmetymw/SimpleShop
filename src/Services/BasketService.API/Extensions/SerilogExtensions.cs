namespace BasketService.API.Extensions;
public static class SerilogExtensions
{
    public static void AddSerilogConfiguration(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        services.AddSingleton(Log.Logger);
    }
}
