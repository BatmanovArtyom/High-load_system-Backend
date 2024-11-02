namespace RateLimiter.Writer.Models.DomainModels;

public class RateLimit
{
    public string Route { get; set; }
    public int RequestsPerMinute { get; set; }
}