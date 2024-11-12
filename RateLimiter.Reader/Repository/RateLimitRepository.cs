using MongoDB.Driver;
using RateLimiter.Reader.Database;
using RateLimiter.Reader.Models.DbModels;

namespace RateLimiter.Reader.Repository;

public class RateLimitRepository : IRateLimitRepository
{
    private readonly IMongoCollection<ReaderDbModel> _collection;


    public RateLimitRepository(DatabaseInitializer dbInitializer)
    {
        _collection = dbInitializer.GetRateLimitCollection();
    }

    public async Task<List<ReaderDbModel>?> GetAllRateLimitsAsync()
    {
        try
        {
            var filter = Builders<ReaderDbModel>.Filter.Empty;
            var rateLimits = await _collection.Find(filter).ToListAsync();
            return rateLimits;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<ReaderDbModel?> GetRateLimitByRouteAsync(string route)
    {
        try
        {
            var filter = Builders<ReaderDbModel>.Filter.Eq(rl => rl.Route, route);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<bool> UpdateRateLimitAsync(ReaderDbModel rateLimit)
    {
        try
        {
            var filter = Builders<ReaderDbModel>.Filter.Eq(rl => rl.Route, rateLimit.Route);
            var update = Builders<ReaderDbModel>.Update
                .Set(rl => rl.RequestsPerMinute, rateLimit.RequestsPerMinute);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    
    public IChangeStreamCursor<ChangeStreamDocument<ReaderDbModel>> WatchChangeStream(
        PipelineDefinition<ChangeStreamDocument<ReaderDbModel>, ChangeStreamDocument<ReaderDbModel>> pipeline)
    {
        var options = new ChangeStreamOptions
        {
            FullDocument = ChangeStreamFullDocumentOption.UpdateLookup
        };
        
        return _collection.Watch(pipeline, options);
    }
}