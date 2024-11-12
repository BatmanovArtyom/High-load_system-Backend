using MongoDB.Driver;
using RateLimiter.Reader.Models.DbModels;
using RateLimiter.Reader.Repository;

namespace RateLimiter.Reader.DomainService;

public class RateLimitLoader
{
    private readonly IRateLimitRepository _rateLimitRepository;
    private readonly ILogger<ReaderService> _logger;

    public RateLimitLoader(IRateLimitRepository rateLimitRepository, ILogger<ReaderService> logger)
    {
        _rateLimitRepository = rateLimitRepository;
        _logger = logger;
    }

    public async Task<Dictionary<string, RateLimit>> LoadRateLimitsAsync()
    {
        var rateLimits = new Dictionary<string, RateLimit>();
        _logger.LogInformation("Loading rate limits from MongoDB through repository...");
        var rateLimitDocs = await _rateLimitRepository.GetAllRateLimitsAsync();
        
        if (rateLimitDocs == null || rateLimitDocs.Count == 0)
        {
            _logger.LogWarning("No rate limits found in MongoDB.");
            return rateLimits;
        }
        
        foreach (var doc in rateLimitDocs)
        {
            rateLimits[doc.Route] = new RateLimit
            {
                Route = doc.Route,
                RequestsPerMinute = doc.RequestsPerMinute
            };
        }

        _logger.LogInformation($"Loaded {rateLimits.Count} rate limits.");
        return rateLimits;
    }
}