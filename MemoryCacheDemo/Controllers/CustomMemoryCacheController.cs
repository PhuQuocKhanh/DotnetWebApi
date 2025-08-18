using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemoryCacheDemo.CustomInMemoryCache.Service;
using MemoryCacheDemo.InMemoryCache.Data;
using MemoryCacheDemo.Model.InMemoryCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace MemoryCacheDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomMemoryCacheController : ControllerBase
    {
         // DbContext để thao tác dữ liệu.
        private readonly ApplicationDbContext _context;

        // CacheManager tùy chỉnh để quản lý cache.
        private readonly CacheManager _cache;

        // Thời gian hết hạn cache.
        private readonly int _CacheAbsoluteDurationMinutes;
        private readonly int _CacheSlidingDurationMinutes;

        // Đọc cấu hình từ appsettings.json.
        private readonly IConfiguration _configuration;

        // Constructor: inject dependencies + đọc settings từ configuration.
        public CustomMemoryCacheController(ApplicationDbContext context, 
                                  CacheManager cache, 
                                  IConfiguration configuration)
        {
            _context = context;
            _cache = cache;
            _configuration = configuration;

            // Đọc thời gian hết hạn cache, nếu không có thì fallback mặc định.
            _CacheAbsoluteDurationMinutes = _configuration.GetValue<int?>("CacheSettings:CacheAbsoluteDurationMinutes") ?? 30;
            _CacheSlidingDurationMinutes = _configuration.GetValue<int?>("CacheSettings:CacheSlidingDurationMinutes") ?? 30;
        }

        // Lấy danh sách Countries với cache.
        public async Task<List<Country>> GetCountriesAsync()
        {
            var cacheKey = "Countries";

            if (!_cache.TryGetValue(cacheKey, out List<Country>? countries))
            {
                // Lấy từ DB nếu chưa có trong cache.
                countries = await _context.Countries.AsNoTracking().ToListAsync();

                // Đặt options cache với High priority (quan trọng).
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetPriority(CacheItemPriority.High);

                // Set vào cache.
                _cache.Set(cacheKey, countries, cacheEntryOptions);
            }
            return countries ?? new List<Country>();
        }

        // Xóa cache Countries (sau khi insert/update).
        public void RemoveCountriesFromCache()
        {
            var cacheKey = "Countries";
            _cache.Remove(cacheKey);
        }

        // Thêm mới Country và clear cache.
        public async Task AddCountry(Country country)
        {
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();
            RemoveCountriesFromCache();
        }

        // Update Country và clear cache.
        public async Task UpdateCountry(Country updatedCountry)
        {
            _context.Countries.Update(updatedCountry);
            await _context.SaveChangesAsync();
            RemoveCountriesFromCache();
        }

        // Lấy danh sách States theo CountryId với Sliding Expiration.
        public async Task<List<State>> GetStatesAsync(int countryId)
        {
            string cacheKey = $"States_{countryId}";

            if (!_cache.TryGetValue(cacheKey, out List<State>? states))
            {
                states = await _context.States
                                       .Where(s => s.CountryId == countryId)
                                       .AsNoTracking()
                                       .ToListAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(_CacheSlidingDurationMinutes))
                    .SetPriority(CacheItemPriority.Normal);

                _cache.Set(cacheKey, states, cacheEntryOptions);
            }
            return states ?? new List<State>();
        }

        // Lấy danh sách Cities theo StateId với Absolute Expiration.
        public async Task<List<City>> GetCitiesAsync(int stateId)
        {
            string cacheKey = $"Cities_{stateId}";

            if (!_cache.TryGetValue(cacheKey, out List<City>? cities))
            {
                cities = await _context.Cities
                                       .Where(c => c.StateId == stateId)
                                       .AsNoTracking()
                                       .ToListAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(_CacheAbsoluteDurationMinutes))
                    .SetPriority(CacheItemPriority.Low);

                _cache.Set(cacheKey, cities, cacheEntryOptions);
            }
            return cities ?? new List<City>();
        }
    }
}