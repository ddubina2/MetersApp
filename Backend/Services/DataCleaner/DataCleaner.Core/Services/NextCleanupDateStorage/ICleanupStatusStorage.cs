using DataCleaner.Core.Services.NextCleanupDateStorage.Models;

namespace DataCleaner.Core.Services.NextCleanupDateStorage;

public interface ICleanupStatusStorage
{
    Task<CleanupStatus> Get();

    Task Set(CleanupStatus cleanupStatus);
}
