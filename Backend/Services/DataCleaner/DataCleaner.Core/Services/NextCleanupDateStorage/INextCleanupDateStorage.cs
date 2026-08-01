namespace DataCleaner.Core.Services.NextCleanupDateStorage;

public interface INextCleanupDateStorage
{
    Task<DateTime> Get();

    Task Set(DateTime date);
}
