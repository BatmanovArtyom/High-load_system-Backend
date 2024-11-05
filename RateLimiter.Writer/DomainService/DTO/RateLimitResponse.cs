namespace RateLimiter.Writer.DomainService.DTO;

public class RateLimitResponse
{
    public string Route { get; set; } = string.Empty;
    public int RequestsPerMinute { get; set; }
}