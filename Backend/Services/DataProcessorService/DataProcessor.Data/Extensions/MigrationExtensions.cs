using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DataProcessor.Data.Extensions;

public static class MigrationExtensions
{
    public static async Task MigrateDbAsync<TDbContext>(
        this IHost host) where TDbContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        var logger = services.GetRequiredService<ILogger<TDbContext>>();
        var dbContext = services.GetRequiredService<TDbContext>();

        try
        {
            logger.LogInformation("Applying EF Core migrations for {DbContext}", typeof(TDbContext).Name);
            await dbContext.Database.MigrateAsync();
            logger.LogInformation("Database migrations applied successfully");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Database migration failed");
            throw;
        }
    }
}
