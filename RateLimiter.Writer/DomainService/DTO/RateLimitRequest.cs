namespace RateLimiter.Writer.DomainService.DTO;

public class RateLimitRequest
{
    public string Route { get; set; } = string.Empty;
    public int RequestsPerMinute { get; set; }
}