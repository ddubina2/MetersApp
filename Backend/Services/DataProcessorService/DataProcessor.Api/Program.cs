namespace DataProcessor.Api;

using DataProcessor.Core.Consumers;
using DataProcessor.Data;
using DataProcessor.Data.Extensions;
using MassTransit;
using MetersApp.Shared.Constants;
using MetersApp.Shared.Middlewares;
using MetersApp.Shared.Options;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

// smth changed
public static class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
            .WriteTo.Console()
            .CreateLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSerilog();

            builder.Services.Configure<DbMigrationsOptions>(builder.Configuration.GetSection(nameof(DbMigrationsOptions)));

            builder.Services.AddDbContext<DataProcessorDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(nameof(RabbitMqOptions)));
            builder.Services.AddMassTransit(x =>
            {
                var options = builder.Configuration.GetSection(nameof(RabbitMqOptions)).Get<RabbitMqOptions>();

                x.AddConsumer<SensorDataBatchConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(options?.Host, "/", h =>
                    {
                        h.Username(options?.UserName ?? string.Empty);
                        h.Password(options?.Password ?? string.Empty);
                    });

                    cfg.ReceiveEndpoint(QueueNames.ProcessSensorDataQueue, e =>
                    {
                        e.ConfigureConsumer<SensorDataBatchConsumer>(context);
                    });
                });
            });

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseSerilogRequestLogging();

            var dbMigrationsOptions = app.Configuration.GetSection(nameof(DbMigrationsOptions)).Get<DbMigrationsOptions>();
            if (dbMigrationsOptions is { RunOnStartup: true })
            {
                await app.MigrateDbAsync<DataProcessorDbContext>(
                    dbMigrationsOptions.MaxRetries, dbMigrationsOptions.DelaySeconds);
            }

            await app.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}
