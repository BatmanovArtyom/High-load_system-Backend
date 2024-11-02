namespace RateLimiter.Writer.Models.DbModels;

public class RateLimitDbModel
{
    public int Id { get; set; }
    public string Route { get; set; }
    public int RequestsPerMinute { get; set; }
}