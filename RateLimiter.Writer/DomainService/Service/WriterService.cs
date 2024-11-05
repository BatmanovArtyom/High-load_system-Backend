using Grpc.Core;
using RateLimiter.Writer.Models.DomainModels;
using RateLimiter.Writer.Repository;

namespace RateLimiter.Writer.DomainService.Service;

public class WriterService : Writer.WriterBase
{
    private readonly IRateLimitRepository _rateLimitRepository;

    public WriterService(IRateLimitRepository rateLimitRepository)
    {
        _rateLimitRepository = rateLimitRepository;
    }

    public override async Task<CreateRateLimitResponse> CreateRateLimit(CreateRateLimitRequest request, ServerCallContext context)
    {
        var rateLimit = new RateLimit
        {
            Route = request.Route,
            RequestsPerMinute = request.RequestsPerMinute
        };

        var success = await _rateLimitRepository.CreateAsync(rateLimit, context.CancellationToken);
        return new CreateRateLimitResponse { Success = success };
    }

    public override async Task<GetRateLimitResponse> GetRateLimit(GetRateLimitRequest request, ServerCallContext context)
    {
        var rateLimit = await _rateLimitRepository.GetByRouteAsync(request.Route, context.CancellationToken);

        if (rateLimit == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Rate limit not found"));

        return new GetRateLimitResponse
        {
            Route = rateLimit.Route,
            RequestsPerMinute = rateLimit.RequestsPerMinute
        };
    }

    public override async Task<UpdateRateLimitResponse> UpdateRateLimit(UpdateRateLimitRequest request, ServerCallContext context)
    {
        var rateLimit = new RateLimit
        {
            Route = request.Route,
            RequestsPerMinute = request.RequestsPerMinute
        };

        var success = await _rateLimitRepository.UpdateAsync(rateLimit, context.CancellationToken);
        return new UpdateRateLimitResponse { Success = success };
    }

    public override async Task<DeleteRateLimitResponse> DeleteRateLimit(DeleteRateLimitRequest request, ServerCallContext context)
    {
        var success = await _rateLimitRepository.DeleteAsync(request.Route, context.CancellationToken);
        return new DeleteRateLimitResponse { Success = success };
    }
}