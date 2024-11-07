namespace RateLimiter.Writer.Models.mapper;

public class RateLimitMapperFactory
{
    public static IRateLimitMapper CreateMapper() => new RateLimitMapper();
}