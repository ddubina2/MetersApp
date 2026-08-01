using DataCleaner.Core.Services.SensorDataCleanup.Models;
using DataCleaner.Data;
using Microsoft.EntityFrameworkCore;

namespace DataCleaner.Core.Services.SensorDataCleanup;

public class SensorDataCleanupService : ISensorDataCleanupService
{
    private readonly DataCleanerDbContext _dbContext;

    public SensorDataCleanupService(DataCleanerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CleanupResult> DeleteOldSensorDataAsync(
        DateTime olderThan,
        CancellationToken cancellationToken = default)
    {
        var airQualityDeleted = await _dbContext.AirQualityReadings
            .Where(x => x.Timestamp < olderThan)
            .ExecuteDeleteAsync(cancellationToken);

        var energyDeleted = await _dbContext.EnergyReadings
            .Where(x => x.Timestamp < olderThan)
            .ExecuteDeleteAsync(cancellationToken);

        var motionDeleted = await _dbContext.MotionReadings
            .Where(x => x.Timestamp < olderThan)
            .ExecuteDeleteAsync(cancellationToken);

        return new CleanupResult
        {
            AirQualityDeletedCount = airQualityDeleted,
            EnergyDeletedCount = energyDeleted,
            MotionDeletedCount = motionDeleted,
        };
    }
}
