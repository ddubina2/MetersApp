using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace DataCleaner.Core.Services.NextCleanupDateStorage;

public class CacheNextCleanupDateStorage : INextCleanupDateStorage
{
    private const string NextCleanupDateCacheKey = "NextCleanupDate";
    private readonly ILogger<CacheNextCleanupDateStorage> _logger;
    private readonly IMemoryCache _memoryCache;

    public CacheNextCleanupDateStorage(ILogger<CacheNextCleanupDateStorage> logger, IMemoryCache memoryCache)
    {
        _logger = logger;
        _memoryCache = memoryCache;
    }

    public Task<DateTime> Get()
    {
        var date = _memoryCache.Get<DateTime>(NextCleanupDateCacheKey);

        return Task.FromResult(date);
    }

    public Task Set(DateTime date)
    {
        _memoryCache.Set(NextCleanupDateCacheKey, date);

        _logger.LogInformation("Next cleanup date was set to {CleanupDate}", date);

        return Task.CompletedTask;
    }
}
