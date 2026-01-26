namespace DataProcessor.Data.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public static class MigrationExtensions
{
    public static async Task MigrateDbAsync<TDbContext>(
        this IHost host,
        int maxRetries = 5,
        int delaySeconds = 5)
        where TDbContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TDbContext>>();
        var dbContext = services.GetRequiredService<TDbContext>();
        var attempt = 0;

        while (true)
        {
            try
            {
                attempt++;
                logger.LogInformation(
                    "Applying EF Core migrations for {DbContext}, attempt {Attempt}", typeof(TDbContext).Name, attempt);

                await dbContext.Database.MigrateAsync();

                logger.LogInformation(
                    "Database migrations applied successfully for {DbContext}", typeof(TDbContext).Name);

                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database migration failed for {DbContext} on attempt {Attempt}", typeof(TDbContext).Name, attempt);

                if (attempt >= maxRetries)
                {
                    logger.LogCritical("Max retries reached ({MaxRetries}). Migration aborted.", maxRetries);
                    throw;
                }

                logger.LogInformation("Waiting {DelaySeconds}s before retrying...", delaySeconds);
                await Task.Delay(delaySeconds * 1000);
            }
        }
    }
}
