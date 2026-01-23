using GraphQLGateway.Api.GraphQL.Queries;
using GraphQLGateway.Api.GraphQL.Types;
using GraphQLGateway.Core;
using GraphQLGateway.Data;
using MetersApp.Shared.Options;
using Microsoft.EntityFrameworkCore;

namespace GraphQLGateway.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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
            });;

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        );
        // Add services to the container.
        builder.Services.ConfigureCoreServices(builder.Configuration);

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseCors("AllowFrontend");
        app.UseHttpsRedirection();
        app.MapGraphQL();
        app.Run();
    }
}
