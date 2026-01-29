namespace GraphQLGateway.Api;

using GraphQLGateway.Api.GraphQL.Queries;
using GraphQLGateway.Api.GraphQL.Types;
using GraphQLGateway.Core;
using GraphQLGateway.Data;
using MetersApp.Shared.Middlewares;
using MetersApp.Shared.Options;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

public static class Program
{
    public static void Main(string[] args)
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

            builder.Services.Configure<CorsOptions>(builder.Configuration.GetSection(nameof(CorsOptions)));
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    var corsOptions = builder.Configuration.GetSection(nameof(CorsOptions)).Get<CorsOptions>();
                    policy.WithOrigins(corsOptions?.AllowedOrigins ?? [])
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            builder.Services
                .AddGraphQLServer()
                .AddQueryType(d => d.Name("Query"))
                .AddType<AirQualityReadingType>()
                .AddTypeExtension<AirQualityQueries>()
                .AddTypeExtension<EnergyQueries>()
                .AddTypeExtension<MotionQueries>()
                .AddFiltering()
                .AddSorting()
                .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true)
                .ModifyCostOptions(options =>
                {
                    options.MaxFieldCost = 5_000;
                });

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.ConfigureCoreServices(builder.Configuration);

            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseSerilogRequestLogging();
            app.UseCors("AllowFrontend");
            app.MapGraphQL();
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
