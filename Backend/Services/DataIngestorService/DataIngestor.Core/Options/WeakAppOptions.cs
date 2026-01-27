namespace DataIngestor.Core.Options;

public class WeakAppOptions
{
    public required string BaseUrl { get; set; }

    public required string ApiKey { get; set; }

    public int RequestIntervalSec { get; set; }
}
