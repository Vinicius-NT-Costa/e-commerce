using Serilog;

namespace UserService.Infrastructure.Configuration;

public static class SerilogConfig
{
    public static void ConfigureLogger(IServiceProvider services, LoggerConfiguration loggerConfig, WebApplicationBuilder builder)
    {
        loggerConfig.ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services);
    }
}