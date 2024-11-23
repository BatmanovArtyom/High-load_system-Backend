using MongoDB.Driver;
using RateLimiter.Reader.Models.DbModels;
using RateLimiter.Reader.Models.mapper;
using RateLimiter.Reader.Repository;

namespace RateLimiter.Reader.DomainService;

public class RateLimitWatcher
{
    private readonly IRateLimitRepository _rateLimitRepository;
    private readonly RateLimitMemoryStore _memoryStore;
    private Task<IAsyncCursor<ChangeStreamDocument<ReaderDbModel>>> _cursor;
    private readonly ILogger<RateLimitWatcher> _logger;
    private readonly IRateLimitMapper _rateLimitMapper;

    public RateLimitWatcher(IRateLimitRepository rateLimitRepository, RateLimitMemoryStore memoryStore, ILogger<RateLimitWatcher> logger, IRateLimitMapper rateLimitMapper)
    {
        _rateLimitRepository = rateLimitRepository;
        _memoryStore = memoryStore;
        _logger = logger;
        _rateLimitMapper = rateLimitMapper;
    }

    public async Task StartWatching()
    {
        var changeStreamOptions = new ChangeStreamOptions
        {
            FullDocument = ChangeStreamFullDocumentOption.UpdateLookup
        };
        
       var _cursor = await _rateLimitRepository.WatchRateLimitChangesAsync(changeStreamOptions);

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
                            var domainRateLimit = _rateLimitMapper.MapToDomainModel(newLimit);
                            _logger.LogInformation("A new entry in the database has been identified");
                            _memoryStore.AddOrUpdateSingle(domainRateLimit);
                        }
                    }

                    if (change.OperationType == ChangeStreamOperationType.Delete)
                    {
                        if (change.DocumentKey.TryGetValue("_id", out var id))
                        {
                            _logger.LogInformation("A delete operation occurred. Removing from memory store.");
                            _memoryStore.RemoveById(id.AsObjectId);
                        }
                    }
                }
            }
        });
    }

    public void StopWatching() => _cursor?.Dispose();

}
