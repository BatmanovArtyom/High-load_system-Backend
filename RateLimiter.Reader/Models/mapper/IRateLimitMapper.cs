using RateLimiter.Reader.Models.DbModels;
using RateLimiter.Reader.Models.DomainModels;

namespace RateLimiter.Reader.Models.mapper;

public interface IRateLimitMapper
{
    public RateLimits MapToDomainModel(ReaderDbModel dbModel);
}