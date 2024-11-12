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
        _ = Task.Run(async () =>
        {
            try
            {
                while (true)
                {
                    var rateLimitDocs = await _rateLimitRepository.GetAllRateLimitsAsync();
                    if (rateLimitDocs != null)
                    {
                        foreach (var doc in rateLimitDocs)
                        {
                            if (_rateLimits.ContainsKey(doc.Route))
                            {
                                _rateLimits[doc.Route] = new RateLimit
                                {
                                    Route = doc.Route,
                                    RequestsPerMinute = doc.RequestsPerMinute
                                };
                            }
                            else
                            {
                                _rateLimits.Add(doc.Route, new RateLimit
                                {
                                    Route = doc.Route,
                                    RequestsPerMinute = doc.RequestsPerMinute
                                });
                            }
                        }
                        _logger.LogInformation("Rate limits updated.");
                    }
                    await Task.Delay(10000);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error watching rate limit updates: {ex.Message}");
            }
        });
    }

}
