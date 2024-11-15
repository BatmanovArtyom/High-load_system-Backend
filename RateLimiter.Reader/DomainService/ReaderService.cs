using MongoDB.Driver;
using RateLimiter.Reader.Models.DbModels;
using RateLimiter.Reader.Repository;

namespace RateLimiter.Reader.DomainService;

public class ReaderService : IHostedService
{
    private readonly RateLimitLoader _rateLimitLoader;
    private readonly RateLimitWatcher _rateLimitWatcher;

    public ReaderService(RateLimitLoader rateLimitLoader, RateLimitWatcher rateLimitWatcher)
    {
        _rateLimitLoader = rateLimitLoader;
        _rateLimitWatcher = rateLimitWatcher;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _rateLimitLoader.LoadInitialDataAsync();
        
        _rateLimitWatcher.StartWatching();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _rateLimitWatcher.StopWatching();
        return Task.CompletedTask;
    }
}