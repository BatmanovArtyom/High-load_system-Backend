using Grpc.Core;
using RateLimiter.Writer.DomainService.Service;
using RateLimiter.Writer.Models.DomainModels;

namespace RateLimiter.Writer.Controller;

public class WriterService : Writer.WriterBase
{
    private readonly IWriterDomainService _writerDomainService;

    public WriterService(IWriterDomainService writerDomainService)
    {
        _writerDomainService = writerDomainService;
    }

    public override async Task<CreateRateLimitResponse> CreateRateLimit(CreateRateLimitRequest request, ServerCallContext context)
    {
        var rateLimit = new RateLimit
        {
            Route = request.Route,
            RequestsPerMinute = request.RequestsPerMinute
        };

        var success = await _writerDomainService.CreateRateLimite(rateLimit, context.CancellationToken);
        return new CreateRateLimitResponse { Success = success };
    }

    public override async Task<GetRateLimitResponse> GetRateLimit(GetRateLimitRequest request, ServerCallContext context)
    {
        var rateLimit = await _writerDomainService.GetByRoute(request.Route, context.CancellationToken);

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

        var success = await _writerDomainService.UpdateRoute(rateLimit, context.CancellationToken);
        return new UpdateRateLimitResponse { Success = success };
    }

    public override async Task<DeleteRateLimitResponse> DeleteRateLimit(DeleteRateLimitRequest request, ServerCallContext context)
    {
        var success = await _writerDomainService.DeleteRoute(request.Route, context.CancellationToken);
        return new DeleteRateLimitResponse { Success = success };
    }
}