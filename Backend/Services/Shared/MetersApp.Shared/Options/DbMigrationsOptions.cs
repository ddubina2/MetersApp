namespace MetersApp.Shared.Options;

public class DbMigrationsOptions
{
    public bool RunOnStartup { get; set; }

    public int MaxRetries { get; set; }

    private int _delaySeconds;
    public int DelaySeconds
    {
        get => _delaySeconds > 0 ? _delaySeconds : 5;
        set => _delaySeconds = value;
    }
}
