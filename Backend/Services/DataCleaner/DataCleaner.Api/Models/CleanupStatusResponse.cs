using MetersApp.Shared.Enums;

namespace DataCleaner.Api.Models;

public record CleanupStatusResponse
{
    public CleaningResult LastCleaningResult { get; set; }

    public DateTime NextCleanup { get; set; }
}
