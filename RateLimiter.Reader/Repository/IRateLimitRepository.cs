using MongoDB.Driver;
using RateLimiter.Reader.Models.DbModels;

namespace RateLimiter.Reader.Repository;

public interface IRateLimitRepository
{
    public Task<List<ReaderDbModel>> GetRateLimitsBatchAsync(int skip, int limit);

    public Task<IAsyncCursor<ChangeStreamDocument<ReaderDbModel>>> WatchRateLimitChangesAsync();
}