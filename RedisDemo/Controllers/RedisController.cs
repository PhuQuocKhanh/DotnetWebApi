using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace RedisDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RedisController : ControllerBase
    {
          // Dùng để thao tác với Redis thông qua IDistributedCache
        private readonly IDistributedCache _distributedCache;
        // Dùng để kết nối trực tiếp với Redis Server
        private readonly IConnectionMultiplexer _redisConnection;
        // Dùng để đọc cấu hình trong appsettings.json
        private readonly IConfiguration _configuration;

        // Inject IDistributedCache, IConnectionMultiplexer, và IConfiguration
        public RedisController(
            IDistributedCache distributedCache,
            IConnectionMultiplexer redisConnection,
            IConfiguration configuration)
        {
            _distributedCache = distributedCache;
            _redisConnection = redisConnection;
            _configuration = configuration;
        }

        // GET: api/RedisCache/all
        // Lấy tất cả key và value từ Redis
        [HttpGet("all")]
        public async Task<IActionResult> GetAllCachedKeysAndValues()
        {
            try
            {
                var server = _redisConnection.GetServer(_redisConnection.GetEndPoints().First());
                var keys = server.Keys().ToArray();
                string instanceName = _configuration["RedisCacheOptions:InstanceName"] ?? string.Empty;

                var cacheEntries = new List<KeyValuePair<string, string>>();

                foreach (var key in keys)
                {
                    var keyWithoutPrefix = key.ToString().Replace($"{instanceName}", "");
                    var value = await _distributedCache.GetStringAsync(keyWithoutPrefix);
                    cacheEntries.Add(new KeyValuePair<string, string>(keyWithoutPrefix, value ?? "null"));
                }

                return Ok(cacheEntries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Không thể lấy danh sách cache.", error = ex.Message });
            }
        }

        // GET: api/RedisCache/{key}
        // Lấy cache theo key
        [HttpGet("{key}")]
        public async Task<IActionResult> GetCacheEntryByKey(string key)
        {
            try
            {
                var value = await _distributedCache.GetStringAsync(key);
                if (value == null)
                {
                    return NotFound(new { message = "Không tìm thấy cache entry." });
                }
                return Ok(new { Key = key, Value = value });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Không thể lấy cache entry.", error = ex.Message });
            }
        }

        // DELETE: api/RedisCache/all
        // Xóa toàn bộ cache
        [HttpDelete("all")]
        public IActionResult ClearAllCacheEntries()
        {
            try
            {
                var server = _redisConnection.GetServer(_redisConnection.GetEndPoints().First());
                foreach (var key in server.Keys())
                {
                    string instanceName = _configuration["RedisCacheOptions:InstanceName"] ?? string.Empty;
                    var keyWithoutPrefix = key.ToString().Replace($"{instanceName}", "");
                    _distributedCache.Remove(keyWithoutPrefix);
                }
                return Ok(new { message = "Đã xóa toàn bộ cache." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Không thể xóa cache.", error = ex.Message });
            }
        }

        // DELETE: api/RedisCache/{key}
        // Xóa cache theo key
        [HttpDelete("{key}")]
        public async Task<IActionResult> ClearCacheEntryByKey(string key)
        {
            try
            {
                await _distributedCache.RemoveAsync(key);
                return Ok(new { message = $"Đã xóa cache entry '{key}'." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Không thể xóa cache entry.", error = ex.Message });
            }
        }
    }
}