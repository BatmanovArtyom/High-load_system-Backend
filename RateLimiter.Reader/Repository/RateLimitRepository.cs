using MongoDB.Bson;
using MongoDB.Driver;
using RateLimiter.Reader.Database;
using RateLimiter.Reader.Models.DbModels;
using RateLimiter.Reader.Models.DomainModels;
using RateLimiter.Reader.Models.mapper;

namespace RateLimiter.Reader.Repository;

public class RateLimitRepository:IRateLimitRepository
{
    private readonly IMongoCollection<ReaderDbModel> _collection;
    private readonly IRateLimitMapper _rateLimitMapper;


    public RateLimitRepository(DatabaseInitializer dbInitializer, IRateLimitMapper rateLimitMapper)
    {
        _collection = dbInitializer.GetRateLimitCollection();
        _rateLimitMapper = rateLimitMapper;
    }

    public async IAsyncEnumerable<RateLimits> GetRateLimitsBatchAsync(int batchSize)
    {
        var options = new FindOptions<ReaderDbModel> {BatchSize = batchSize};
        using var cursor = await _collection.FindAsync(FilterDefinition<ReaderDbModel>.Empty, options);

        while (await cursor.MoveNextAsync())
        {
            foreach (var rateLimit in cursor.Current)
            {
                yield return _rateLimitMapper.MapToDomainModel(rateLimit);
            }
        }
    }
    
    public async Task WatchRateLimitChangesAsync(Action<RateLimits> onRateLimitUpdate, Action<ObjectId> onRateLimitDelete)
    {
        var options = new ChangeStreamOptions
        {
            FullDocument = ChangeStreamFullDocumentOption.UpdateLookup
        };

        using var cursor = await _collection.WatchAsync(options);
        await cursor.ForEachAsync(change =>
        {
            if (change.OperationType == ChangeStreamOperationType.Update ||
                change.OperationType == ChangeStreamOperationType.Insert)
            {
                var updatedRateLimit = new RateLimits
                {
                    Id = change.DocumentKey["_id"].AsObjectId,
                    Route = change.FullDocument.Route,
                    RequestsPerMinute = change.FullDocument.RequestsPerMinute
                };
                onRateLimitUpdate(updatedRateLimit);
            }
            else if (change.OperationType == ChangeStreamOperationType.Delete)
            {
                var deletedId = change.DocumentKey["_id"].AsObjectId;
                onRateLimitDelete(deletedId);
            }
        });
    }
}    