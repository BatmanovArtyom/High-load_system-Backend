using MongoDB.Driver;
using RateLimiter.Reader.Models.DbModels;

namespace RateLimiter.Reader.Repository;

public interface IRateLimitRepository
{
    public IAsyncEnumerable<ReaderDbModel> GetRateLimitsBatchAsync(int batchSize);

    public Task<IAsyncCursor<ChangeStreamDocument<ReaderDbModel>>> WatchRateLimitChangesAsync(ChangeStreamOptions options);
}