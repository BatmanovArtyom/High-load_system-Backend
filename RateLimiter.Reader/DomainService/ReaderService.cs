using MongoDB.Driver;
using RateLimiter.Reader.Models.DbModels;
using RateLimiter.Reader.Repository;

namespace RateLimiter.Reader.DomainService;

public class ReaderService
{
    private Dictionary<string, RateLimit> _rateLimits = new();
    private readonly ILogger<ReaderService> _logger;
    private readonly RateLimitLoader _rateLimitLoader;
    private readonly RateLimitWatcher _rateLimitWatcher;
    private readonly IRateLimitRepository _rateLimitRepository;

    public ReaderService(
        ILogger<ReaderService> logger, IRateLimitRepository rateLimitRepository)
    {
        _rateLimitRepository = rateLimitRepository;
        _rateLimitLoader = new RateLimitLoader(_rateLimitRepository, logger);
        _rateLimitWatcher = new RateLimitWatcher(_rateLimitRepository, _rateLimits, logger);

        LoadRateLimitsAsync().Wait(); 
        _rateLimitWatcher.WatchRateLimitUpdates();
    }

    private async Task LoadRateLimitsAsync()
    {
        _rateLimits.Clear();
        _rateLimits = await _rateLimitLoader.LoadRateLimitsAsync();
    }

    public List<RateLimit> GetAllRateLimits()
    {
        return _rateLimits.Values.ToList();
    }
}