using MongoDB.Driver;
using RateLimiter.Reader.Models.DbModels;

namespace RateLimiter.Reader.Repository;

public interface IRateLimitRepository
{
    Task<List<ReaderDbModel>?> GetAllRateLimitsAsync();
    Task<ReaderDbModel?> GetRateLimitByRouteAsync(string route);
    Task<bool> UpdateRateLimitAsync(ReaderDbModel rateLimit);

    public IChangeStreamCursor<ChangeStreamDocument<ReaderDbModel>> WatchChangeStream(
        PipelineDefinition<ChangeStreamDocument<ReaderDbModel>, ChangeStreamDocument<ReaderDbModel>> pipeline);
}