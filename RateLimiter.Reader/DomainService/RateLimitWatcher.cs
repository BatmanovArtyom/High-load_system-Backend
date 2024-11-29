using MongoDB.Driver;
using RateLimiter.Reader.Models.DbModels;
using RateLimiter.Reader.Models.mapper;
using RateLimiter.Reader.Repository;

namespace RateLimiter.Reader.DomainService;

public class RateLimitWatcher : IDisposable
{
    private readonly IRateLimitRepository _rateLimitRepository;
    private readonly RateLimitMemoryStore _memoryStore;
    private Task<IAsyncCursor<ChangeStreamDocument<ReaderDbModel>>> _cursor;
    private readonly ILogger<RateLimitWatcher> _logger;

    public RateLimitWatcher(IRateLimitRepository rateLimitRepository, RateLimitMemoryStore memoryStore, ILogger<RateLimitWatcher> logger, Task<IAsyncCursor<ChangeStreamDocument<ReaderDbModel>>> cursor)
    {
        _rateLimitRepository = rateLimitRepository;
        _memoryStore = memoryStore;
        _logger = logger;
        _cursor = cursor;
    }

    public async Task StartWatching()
    {
        try
        {
            await _rateLimitRepository.WatchRateLimitChangesAsync(
                onRateLimitUpdate: updatedRateLimit =>
                {
                    _logger.LogInformation("A new or updated entry in the database has been identified");
                    _memoryStore.AddOrUpdateSingle(updatedRateLimit);
                },
                onRateLimitDelete: deletedId =>
                {
                    _logger.LogInformation("A delete operation occurred. Removing from memory store.");
                    _memoryStore.RemoveById(deletedId);
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start watching change stream");
        }
    }
    public void Dispose() => _cursor?.Dispose();
}
