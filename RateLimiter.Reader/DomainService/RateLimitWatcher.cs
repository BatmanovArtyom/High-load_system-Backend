using MongoDB.Driver;
using RateLimiter.Reader.Models.DbModels;
using RateLimiter.Reader.Repository;

namespace RateLimiter.Reader.DomainService;

public class RateLimitWatcher
{
    private readonly IRateLimitRepository _rateLimitRepository;
    private readonly Dictionary<string, RateLimit> _rateLimits;
    private readonly ILogger<ReaderService> _logger;

    public RateLimitWatcher(IRateLimitRepository rateLimitRepository,
        Dictionary<string, RateLimit> rateLimits,
        ILogger<ReaderService> logger)
    {
        _rateLimitRepository = rateLimitRepository;
        _rateLimits = rateLimits;
        _logger = logger;
    }

    public void WatchRateLimitUpdates()
    {
        var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<ReaderDbModel>>()
            .Match(change => change.OperationType == ChangeStreamOperationType.Update);

        var changeStream = _rateLimitRepository.WatchChangeStream(pipeline);

        _ = Task.Run(async () =>
        {
            try
            {
                while (await changeStream.MoveNextAsync())
                {
                    foreach (var change in changeStream.Current)
                    {
                        var updatedDoc = change.FullDocument;

                        if (updatedDoc == null) continue;
                        
                        _rateLimits[updatedDoc.Route] = new RateLimit
                        {
                            Route = updatedDoc.Route,
                            RequestsPerMinute = updatedDoc.RequestsPerMinute
                        };

                        _logger.LogInformation($"Updated rate limit for route: {updatedDoc.Route}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error watching rate limit updates: {ex.Message}");
            }
        });
    }
}
