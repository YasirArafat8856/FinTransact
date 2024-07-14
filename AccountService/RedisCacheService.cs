using StackExchange.Redis;

namespace AccountService
{
    public class RedisCacheService
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task SetCacheAsync(string key, string value)
        {
            var db = _redis.GetDatabase();
            await db.StringSetAsync(key, value);
        }

        public async Task<string> GetCacheAsync(string key)
        {
            var db = _redis.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async Task RemoveCacheAsync(string key)
        {
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync(key);
        }
    }
}
