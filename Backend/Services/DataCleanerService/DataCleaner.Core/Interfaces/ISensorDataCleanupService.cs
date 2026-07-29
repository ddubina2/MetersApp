namespace DataCleaner.Core.Interfaces;

public interface ISensorDataCleanupService
{
    Task DeleteOldSensorDataAsync(DateTime olderThan, CancellationToken cancellationToken = default);
}
