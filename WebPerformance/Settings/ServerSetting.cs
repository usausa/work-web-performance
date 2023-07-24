namespace WebPerformance.Settings;

public class RateLimitSetting
{
    public int Window { get; set; }

    public int PermitLimit { get; set; }

    public int QueueLimit { get; set; }
}

public class ServerSetting
{
    public RateLimitSetting RateLimit { get; set; } = default!;
}
