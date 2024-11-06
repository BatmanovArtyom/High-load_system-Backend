using RateLimiter.Writer.Models.DomainModels;

namespace RateLimiter.Writer.DomainService.Service;

public interface IWriterDomainService
{
    public Task<bool> CreateRateLimite(RateLimit rateLimit,CancellationToken cancellationToken);
    Task<RateLimit> GetByRoute(string route, CancellationToken cancellationToken);
    Task<bool> UpdateRoute(RateLimit rateLimit, CancellationToken cancellationToken);
    Task<bool> DeleteRoute(string route, CancellationToken cancellationToken);
}