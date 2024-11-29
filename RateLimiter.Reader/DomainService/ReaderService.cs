using MongoDB.Driver;
using RateLimiter.Reader.Models.DbModels;
using RateLimiter.Reader.Repository;

namespace RateLimiter.Reader.DomainService;

public class ReaderService : IHostedService
{
    private const int BatchSize = 3;
    private readonly RateLimitLoader _rateLimitLoader;
    private readonly RateLimitWatcher _rateLimitWatcher;

    public ReaderService(RateLimitLoader rateLimitLoader, RateLimitWatcher rateLimitWatcher)
    {
        _rateLimitLoader = rateLimitLoader;
        _rateLimitWatcher = rateLimitWatcher;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _rateLimitLoader.LoadInitialDataAsync(BatchSize);
        _rateLimitWatcher.StartWatching();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _rateLimitWatcher.Dispose();
        return Task.CompletedTask;
    }
}