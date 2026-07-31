using Serilog;
using Serilog.Events;

namespace MetersApp.Shared.Extensions;

public static class LoggerConfigurationExtensions
{
    public static LoggerConfiguration ConfigureMetersAppLogging(
        this LoggerConfiguration configuration)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var isDevelopment = !string.IsNullOrWhiteSpace(env) &&
                            env.Equals("Development", StringComparison.OrdinalIgnoreCase);

        var logger = configuration
            .Enrich.FromLogContext()
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning);

        if (isDevelopment)
        {
            return logger.WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}");
        }

        return logger
            .MinimumLevel.Is(LogEventLevel.Error)
            .WriteTo.Console();
    }
}
