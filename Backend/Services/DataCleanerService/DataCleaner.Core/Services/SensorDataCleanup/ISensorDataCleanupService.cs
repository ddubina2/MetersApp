using DataCleaner.Core.Services.SensorDataCleanup.Models;

namespace DataCleaner.Core.Services.SensorDataCleanup;

public interface ISensorDataCleanupService
{
    Task<CleanupResult> DeleteOldSensorDataAsync(DateTime olderThan, CancellationToken cancellationToken = default);
}
