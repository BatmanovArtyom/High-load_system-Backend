using Grpc.Core;
using RateLimiter.Writer.Models.DomainModels;
using RateLimiter.Writer.Repository;

namespace RateLimiter.Writer.DomainService.Service;

public class WriterDomainService :IWriterDomainService
{
    private readonly IRateLimitRepository _rateLimitRepository;
    private readonly ILogger<WriterDomainService> _logger;

    public WriterDomainService(IRateLimitRepository rateLimitRepository, 
        ILogger<WriterDomainService> logger)
    {
        _rateLimitRepository = rateLimitRepository;
        _logger = logger;
    }


    public async Task<bool> CreateRateLimite(RateLimit rateLimit, CancellationToken cancellationToken)
    {
        var existingLimit = await _rateLimitRepository.GetByRouteAsync(rateLimit.Route, cancellationToken);
        if (existingLimit != null)
        {
            _logger.LogWarning("Limit creation failed. Limit already exists.");
            throw new RpcException(new Status(StatusCode.AlreadyExists, "Limit already exists."));
        }
        
        var isCreated = await _rateLimitRepository.CreateAsync(rateLimit, cancellationToken);
        if (isCreated)
        {
            _logger.LogInformation("Limit {Route} create", rateLimit.Route);
        }
        else
        {
            _logger.LogError("Error with creating limit");
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid limite" ));
        }
        
        return isCreated;
    }

    public async Task<RateLimit> GetByRoute(string route, CancellationToken cancellationToken)
    {
        if (route == null )
        {
            _logger.LogError("Error get limit {Route}", route);
            throw new RpcException(new Status(StatusCode.NotFound, $"Not name"));
        }
        var limit = await _rateLimitRepository.GetByRouteAsync(route,  cancellationToken);

        if (limit == null)
        {
            _logger.LogError("Error get limit {Route}", route);
            throw new RpcException(new Status(StatusCode.NotFound,
                $"Limit with route {route} not found."));
        }
        
        return limit; 
    }

    public async Task<bool> UpdateRoute(RateLimit rateLimit, CancellationToken cancellationToken)
    {
        var existingLimit = await _rateLimitRepository.GetByRouteAsync(rateLimit.Route, cancellationToken);
            
        if (existingLimit == null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Limit with route {rateLimit.Route} not found."));

        existingLimit.Route = rateLimit.Route;
        existingLimit.RequestsPerMinute = rateLimit.RequestsPerMinute;

        var isUpdated = await _rateLimitRepository.UpdateAsync(existingLimit, cancellationToken);
        if (isUpdated)
        {
            _logger.LogInformation("Limit {route} update", rateLimit.Route);
        }
        else
        {
            _logger.LogError("Error update limit {route}", rateLimit.Route);
        }
        
        return isUpdated;
    }

    public async Task<bool> DeleteRoute(string route, CancellationToken cancellationToken)
    {
        var isDeleted = await _rateLimitRepository.DeleteAsync(route, cancellationToken);
        if (isDeleted)
        {
            _logger.LogInformation("limit with route {route} deleted successfully.", route);
        }
        else
        {
            _logger.LogError("Failed to delete user with ID {route}.",  route);
        }
        
        return isDeleted;
    }
}