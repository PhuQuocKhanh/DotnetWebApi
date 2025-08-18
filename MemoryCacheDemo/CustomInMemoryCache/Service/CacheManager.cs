using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace MemoryCacheDemo.CustomInMemoryCache.Service
{
    public class CacheManager
    {
         // Giữ một instance của IMemoryCache để thực hiện các thao tác cache
        private readonly IMemoryCache _cache;

        // Sử dụng ConcurrentDictionary (thread-safe) để theo dõi các cache key
        private readonly ConcurrentDictionary<string, bool> _cacheKeys;

        // Constructor nhận IMemoryCache từ Dependency Injection
        public CacheManager(IMemoryCache cache)
        {
            _cache = cache;
            _cacheKeys = new ConcurrentDictionary<string, bool>();
        }

        // Thêm một entry vào cache và track key trong ConcurrentDictionary
        // options định nghĩa chiến lược expiration, priority, v.v.
        public void Set<T>(string key, T value, MemoryCacheEntryOptions options)
        {
            _cache.Set(key, value, options);
            _cacheKeys.TryAdd(key, true);
        }

        // Lấy một entry từ cache
        // Nếu tồn tại trong cache thì trả về true và gán vào value
        // Nếu không, xóa key khỏi dictionary
        public bool TryGetValue<T>(string key, out T? value)
        {
            if (_cache.TryGetValue(key, out value))
            {
                return true;
            }
            _cacheKeys.TryRemove(key, out _);
            value = default;
            return false;
        }

        // Xóa một entry khỏi cả IMemoryCache và dictionary
        public void Remove(string key)
        {
            _cache.Remove(key);
            _cacheKeys.TryRemove(key, out _);
        }

        // Lấy danh sách tất cả cache keys đang được track
        // Lưu ý: có thể chứa những key đã expired, nên cần re-check trong IMemoryCache
        public List<string> GetAllKeys()
        {
            return _cacheKeys.Keys.ToList();
        }

        // Xóa toàn bộ cache entries và reset dictionary
        public void Clear()
        {
            foreach (var key in _cacheKeys.Keys)
            {
                _cache.Remove(key);
            }
            _cacheKeys.Clear();
        }
        
    }
}