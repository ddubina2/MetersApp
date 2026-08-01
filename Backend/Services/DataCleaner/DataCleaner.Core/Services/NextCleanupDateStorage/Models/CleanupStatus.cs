using MetersApp.Shared.Enums;

namespace DataCleaner.Core.Services.NextCleanupDateStorage.Models;

public class CleanupStatus
{
    public required CleaningResult LastCleaningResult { get; set; }

    public required DateTime NextCleanupDate { get; set; }
}
