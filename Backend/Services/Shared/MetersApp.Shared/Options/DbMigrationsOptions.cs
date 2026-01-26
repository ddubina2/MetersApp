namespace MetersApp.Shared.Options;

public class DbMigrationsOptions
{
    public bool RunOnStartup { get; set; }

    public int MaxRetries { get; set; }

    public int DelaySeconds { get; set; }
}
