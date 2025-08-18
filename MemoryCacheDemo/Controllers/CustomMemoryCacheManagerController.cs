using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemoryCacheDemo.CustomInMemoryCache.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace MemoryCacheDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomMemoryCacheManagerController : ControllerBase
    {
        // Service quản lý cache keys và các thao tác với cache.
        private readonly CacheManager _cacheManager;

        // Truy cập trực tiếp IMemoryCache để lấy giá trị cache.
        private readonly IMemoryCache _memoryCache;

        // Constructor sử dụng Dependency Injection.
        public CustomMemoryCacheManagerController(CacheManager cacheManager, IMemoryCache memoryCache)
        {
            _cacheManager = cacheManager;
            _memoryCache = memoryCache;
        }

        // Lấy tất cả cache entries (key + value)
        // GET /api/cache/all
        [HttpGet("All")]
        public IActionResult GetAllCacheEntries()
        {
            var cacheEntries = new List<object>();
            var keys = _cacheManager.GetAllKeys();

            foreach (var key in keys)
            {
                if (_memoryCache.TryGetValue(key, out object? value))
                {
                    cacheEntries.Add(new { Key = key, Value = value });
                }
            }
            return Ok(cacheEntries);
        }

        // Lấy một cache entry cụ thể theo key
        // GET /api/cache/{key}
        [HttpGet("{key}")]
        public IActionResult GetCacheEntry(string key)
        {
            if (_memoryCache.TryGetValue(key, out object? value))
            {
                return Ok(new { Key = key, Value = value });
            }
            return NotFound(new { Message = $"Cache key '{key}' không tồn tại." });
        }

        // Xóa toàn bộ cache
        // DELETE /api/cache/clearall
        [HttpDelete("ClearAll")]
        public IActionResult ClearAllCaches()
        {
            _cacheManager.Clear();
            return Ok(new { Message = "Tất cả cache entries đã bị xóa." });
        }

        // Xóa cache entry theo key
        // DELETE /api/cache/{key}
        [HttpDelete("{key}")]
        public IActionResult ClearCacheByKey(string key)
        {
            if (_cacheManager.GetAllKeys().Contains(key))
            {
                _cacheManager.Remove(key);
                return Ok(new { Message = $"Cache entry '{key}' đã bị xóa." });
            }
            else
            {
                return NotFound(new { Message = $"Cache key '{key}' không tồn tại." });
            }
        }
    }
}