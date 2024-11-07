using RateLimiter.Writer.Models.DbModels;
using RateLimiter.Writer.Models.DomainModels;

namespace RateLimiter.Writer.Models.mapper;

public class RateLimitMapper : IRateLimitMapper
{
    public RateLimit? MapToRateLimitFromDbModel(RateLimitDbModel? rateLimitDb)
    {
        if (rateLimitDb != null)
        {
            return new RateLimit
            {
                Id = rateLimitDb.Id,
                Route = rateLimitDb.Route,
                RequestsPerMinute = rateLimitDb.RequestsPerMinute

            };
        }
        return null;
    }

    public RateLimitDbModel? MapToDbModelFromRateLimit(RateLimit? rateLimit)
    {
        if (rateLimit != null)
            return new RateLimitDbModel
            {
                Id = rateLimit.Id,
                Route = rateLimit.Route,
                RequestsPerMinute = rateLimit.RequestsPerMinute
            };
        return null;
    }
}