using GraphQLServer.Api.GraphQL.Queries;
using GraphQLServer.Api.GraphQL.Types;
using GraphQLServer.Core;
using GraphQLServer.Data;
using MetersApp.Shared.Extensions;
using MetersApp.Shared.Middlewares;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace GraphQLServer.Api;

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
                "graphql-server",
                "GraphQLServer.Metrics");

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
