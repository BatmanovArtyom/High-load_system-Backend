using RateLimiter.Writer.Models.DbModels;
using RateLimiter.Writer.Models.DomainModels;

namespace RateLimiter.Writer.Models.mapper;

public interface IRateLimitMapper
{
    RateLimit? MapToRateLimitFromDbModel(RateLimitDbModel rateLimitDb);
    RateLimitDbModel? MapToDbModelFromRateLimit(RateLimit? rateLimit);
}