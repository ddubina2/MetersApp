using DataCleaner.Core.Services.NextCleanupDateStorage.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace DataCleaner.Core.Services.NextCleanupDateStorage;

public class CacheCleanupStatusStorage : ICleanupStatusStorage
{
    private const string NextCleanupDateCacheKey = "NextCleanupDate";
    private readonly ILogger<CacheCleanupStatusStorage> _logger;
    private readonly IMemoryCache _memoryCache;

    public CacheCleanupStatusStorage(ILogger<CacheCleanupStatusStorage> logger, IMemoryCache memoryCache)
    {
        _logger = logger;
        _memoryCache = memoryCache;
    }

    public Task<CleanupStatus> Get()
    {
        var status = _memoryCache.Get<CleanupStatus>(NextCleanupDateCacheKey);

        return status == null
            ? throw new InvalidOperationException("Cache is empty")
            : Task.FromResult(status);
    }

    public Task Set(CleanupStatus cleanupStatus)
    {
        _memoryCache.Set(NextCleanupDateCacheKey, cleanupStatus);

        _logger.LogInformation("Next cleanup date was set to {CleanupDate}", cleanupStatus.NextCleanupDate);

        return Task.CompletedTask;
    }
}
