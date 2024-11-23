using MongoDB.Driver;
using RateLimiter.Reader.Database;
using RateLimiter.Reader.Models.DbModels;

namespace RateLimiter.Reader.Repository;

public class RateLimitRepository:IRateLimitRepository
{
    private readonly IMongoCollection<ReaderDbModel> _collection;


    public RateLimitRepository(DatabaseInitializer dbInitializer)
    {
        _collection = dbInitializer.GetRateLimitCollection();
    }

    public async IAsyncEnumerable<ReaderDbModel> GetRateLimitsBatchAsync(int batchSize)
    {
        var options = new FindOptions<ReaderDbModel> {BatchSize = batchSize};
        using var cursor = await _collection.FindAsync(FilterDefinition<ReaderDbModel>.Empty, options);

        while (await cursor.MoveNextAsync())
        {
            foreach (var rateLimit in cursor.Current)
            {
                yield return rateLimit;
            }
        }
    }
    
    public async Task<IAsyncCursor<ChangeStreamDocument<ReaderDbModel>>> WatchRateLimitChangesAsync(ChangeStreamOptions options)
    {
        var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<ReaderDbModel>>()
            .Match(change => change.OperationType == ChangeStreamOperationType.Update ||
                             change.OperationType == ChangeStreamOperationType.Insert ||
                             change.OperationType == ChangeStreamOperationType.Delete);
        
        return await _collection.WatchAsync(pipeline, options);
    }
}    