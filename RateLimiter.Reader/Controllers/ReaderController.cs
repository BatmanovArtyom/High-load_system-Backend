using Grpc.Core;
using RateLimiter.Reader.DomainService;
using RateLimiter.Reader.Models.DbModels;

namespace RateLimiter.Reader.Controllers;

public class ReaderController : RateLimiterService.RateLimiterServiceBase
{
    private readonly RateLimitMemoryStore _memoryStore;

    public ReaderController(RateLimitMemoryStore memoryStore)
    {
        _memoryStore = memoryStore;
    }

    public override Task<RateLimitResponse> GetRateLimits(EmptyRequest request, ServerCallContext context)
    {
        var response = new RateLimitResponse();
        response.RateLimits.AddRange(_memoryStore.GetAllRateLimits().Select(rl => new RateLimit
        {
            Route = rl.Route,
            RequestsPerMinute = rl.RequestsPerMinute
        }));
        return Task.FromResult(response);
    }
}