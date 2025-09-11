using Microsoft.Extensions.Logging;
using Project.Application.Common.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Project.Infrastructure.Persistence.Repositories
{
    internal class CacheRepository : ICacheRepository
    {
        private readonly IDatabase _database;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly ILogger<CacheRepository> _logger;

        public CacheRepository(IConnectionMultiplexer connectionMultiplexer, ILogger<CacheRepository> logger)
        {
            _database = connectionMultiplexer.GetDatabase();
            _logger = logger;


            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };
        }

        public async Task<T?> GetAsync<T>(string key)
        {

            try
            {

                var value = await _database.StringGetAsync(key);

                //--> Redis values
                //Redis itself only stores byte[].
                //When you call StringGetAsync(key), you always get a RedisValue.

                if (value.IsNullOrEmpty)
                    return default;

                return JsonSerializer.Deserialize<T>(value!, _jsonOptions);
            }


            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Redis unavailable during GET. Key={Key}", key);
                return default;
            }

        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {

            try
            {


                var json = JsonSerializer.Serialize(value, _jsonOptions);
                await _database.StringSetAsync(key, json, expiry);
            }


            catch (Exception ex)
            {

                _logger.LogWarning(ex, "Redis unavailable during SET. Key={Key}", key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }
    }
}


// why  we added error handling despite there is a global error handler:
//If Redis is down, the system should still work by falling back to the DB.
//The user should never see 500 just because Redis is unavailable.
