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

    public async Task<List<ReaderDbModel>> GetRateLimitsBatchAsync(int skip, int limit)
    {
        return await _collection.Find(FilterDefinition<ReaderDbModel>.Empty)
            .Skip(skip)
            .Limit(limit)
            .ToListAsync();
    }
    
    public IChangeStreamCursor<ChangeStreamDocument<ReaderDbModel>> WatchRateLimitChanges()
    {
        var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<ReaderDbModel>>()
            .Match(change => change.OperationType == ChangeStreamOperationType.Update || 
                             change.OperationType == ChangeStreamOperationType.Insert);
        return _collection.Watch(pipeline);
    }
}