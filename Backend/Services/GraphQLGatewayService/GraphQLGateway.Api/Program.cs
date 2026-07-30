using GraphQLGateway.Api.GraphQL.Queries;
using GraphQLGateway.Api.GraphQL.Types;
using GraphQLGateway.Core;
using GraphQLGateway.Data;
using MetersApp.Shared.Extensions;
using MetersApp.Shared.Middlewares;
using MetersApp.Shared.Options;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace GraphQLGateway.Api;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .ConfigureMetersAppLogging()
            .CreateLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSerilog();
            builder.Services.ConfigureTelemetry(
                builder.Environment,
                "graphql-gateway",
                "GraphQlGateway.Metrics");

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

            if (app.Environment.IsProduction())
            {
                app.MapPrometheusScrapingEndpoint();
            }

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
