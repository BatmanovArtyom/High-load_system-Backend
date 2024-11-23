using RateLimiter.Reader.Models.DbModels;
using RateLimiter.Reader.Models.DomainModels;

namespace RateLimiter.Reader.Models.mapper;

public class RateLimitMapper:IRateLimitMapper
{
    public RateLimits MapToDomainModel(ReaderDbModel dbModel)
    {
        return new RateLimits
        {
            Id = dbModel.Id,
            Route = dbModel.Route,
            RequestsPerMinute = dbModel.RequestsPerMinute
        };
    }
}