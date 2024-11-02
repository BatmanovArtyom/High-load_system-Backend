using MongoDB.Driver;
using RateLimiter.Writer.Models.DbModels;
using RateLimiter.Writer.Models.DomainModels;
using RateLimiter.Writer.Models.mapper;

namespace RateLimiter.Writer.Repository;

public class RateLimitRepository : IRateLimitRepository
{
    private readonly IMongoCollection<RateLimitDbModel> _rateLimits;
    private readonly IRateLimitMapper _rateLimitMapper;

    public RateLimitRepository(IMongoDatabase database, IRateLimitMapper rateLimitMapper)
    {
        _rateLimits = database.GetCollection<RateLimitDbModel>("rate_limits");
        _rateLimitMapper = rateLimitMapper;
    }
    
    
    public async Task<bool> CreateAsync(RateLimit rateLimit, CancellationToken cancellationToken)
    {
        RateLimitDbModel? rateLimitDb = _rateLimitMapper.MapToDbModelFromRateLimit(rateLimit);
        var existing = await _rateLimits.Find(x => x.Route == rateLimit.Route).FirstOrDefaultAsync(cancellationToken);
        if (existing != null)
        {
            return false;
        }
        await _rateLimits.InsertOneAsync(rateLimitDb, cancellationToken: cancellationToken);
        return true;
    }
    public async Task<RateLimit> GetByRouteAsync(string route, CancellationToken cancellationToken)
    {
        RateLimitDbModel rateLimitDb =  await _rateLimits.Find(x => x.Route == route).FirstOrDefaultAsync(cancellationToken);
        RateLimit? rateLimit = _rateLimitMapper.MapToRateLimitFromDbModel(rateLimitDb);
        return rateLimit;
    }

    public async Task<bool> UpdateAsync(RateLimit  rateLimit, CancellationToken cancellationToken)
    {
        RateLimitDbModel? rateLimitDb = _rateLimitMapper.MapToDbModelFromRateLimit(rateLimit);
        var result = await _rateLimits.ReplaceOneAsync(
            x => x.Route == rateLimit.Route,
            rateLimitDb,
            cancellationToken: cancellationToken
        );

        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string route, CancellationToken cancellationToken)
    {
        var result = await _rateLimits.DeleteOneAsync(x => x.Route == route, cancellationToken);
        return result.DeletedCount > 0;
    }
}