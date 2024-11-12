using Grpc.Core;
using RateLimiter.Reader.DomainService;

namespace RateLimiter.Reader.Controllers;

public class ReaderController : RateLimiterService.RateLimiterServiceBase
{
    private readonly ReaderService _readerService;

    public ReaderController(ReaderService readerService)
    {
        _readerService = readerService;
    }
    
    public override Task<RateLimitResponse> GetRateLimits(EmptyRequest request, ServerCallContext context)
    {
        var rateLimits = _readerService.GetAllRateLimits();
        
        var response = new RateLimitResponse();
        response.RateLimits.AddRange(rateLimits.Select(rl => new RateLimiter.RateLimit
        {
            Route = rl.Route,
            RequestsPerMinute = rl.RequestsPerMinute
        }));

        return Task.FromResult(response);
    }
}