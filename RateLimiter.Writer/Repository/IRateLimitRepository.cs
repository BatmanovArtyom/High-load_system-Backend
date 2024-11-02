using RateLimiter.Writer.Models.DbModels;
using RateLimiter.Writer.Models.DomainModels;

namespace RateLimiter.Writer.Repository;

public interface IRateLimitRepository
{
    Task<bool> CreateAsync(RateLimit rateLimit, CancellationToken cancellationToken);
    Task<RateLimit> GetByRouteAsync(string route, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(RateLimit rateLimit,CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string route,CancellationToken cancellationToken);
}