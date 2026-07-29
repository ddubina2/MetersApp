using DataCleaner.Core.Interfaces;
using DataCleaner.Data;
using Microsoft.EntityFrameworkCore;

namespace DataCleaner.Core.Services;

public class SensorDataCleanupService : ISensorDataCleanupService
{
    private readonly DataCleanerDbContext _dbContext;

    public SensorDataCleanupService(DataCleanerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task DeleteOldSensorDataAsync(DateTime olderThan, CancellationToken cancellationToken = default)
    {
        await _dbContext.AirQualityReadings
            .Where(x => x.Timestamp < olderThan)
            .ExecuteDeleteAsync(cancellationToken);

        await _dbContext.EnergyReadings
            .Where(x => x.Timestamp < olderThan)
            .ExecuteDeleteAsync(cancellationToken);

        await _dbContext.MotionReadings
            .Where(x => x.Timestamp < olderThan)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
