using MongoDB.Driver;
using RateLimiter.Reader.Models.DbModels;
using RateLimiter.Reader.Repository;

namespace RateLimiter.Reader.DomainService;

public class RateLimitWatcher
{
    private readonly IRateLimitRepository _rateLimitRepository;
    private readonly RateLimitMemoryStore _memoryStore;
    private IChangeStreamCursor<ChangeStreamDocument<ReaderDbModel>> _cursor;
    private readonly ILogger<RateLimitWatcher> _logger;

    public RateLimitWatcher(IRateLimitRepository rateLimitRepository, RateLimitMemoryStore memoryStore, ILogger<RateLimitWatcher> logger)
    {
        _rateLimitRepository = rateLimitRepository;
        _memoryStore = memoryStore;
        _logger = logger;
    }

    public void StartWatching()
    {
        _cursor = _rateLimitRepository.WatchRateLimitChanges();

        Task.Run(async () => { 
            while (_cursor != null && await _cursor.MoveNextAsync())
            {
                foreach (var change in _cursor.Current)
                {
                    if (change.OperationType == ChangeStreamOperationType.Update ||
                        change.OperationType == ChangeStreamOperationType.Insert)
                    {
                        var newLimit = change.FullDocument;
                        if (newLimit != null)
                        {
                            _logger.LogInformation("A new entry in the database has been identified");
                            _memoryStore.AddOrUpdateSingle(newLimit);
                        }
                    }
                }
            }
        });
    }

    public void StopWatching() => _cursor?.Dispose();

}
