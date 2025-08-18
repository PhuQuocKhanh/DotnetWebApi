using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemoryCacheDemo.InMemoryCache.Data;
using MemoryCacheDemo.Model.InMemoryCache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace MemoryCacheDemo.InMemoryCache
{
    public class LocationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        public LocationRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        // Countries: Manual eviction + High Priority
        public async Task<List<Country>> GetCountriesAsync()
        {
            var cacheKey = "Countries";
            if (!_cache.TryGetValue(cacheKey, out List<Country>? countries))
            {
                countries = await _context.Countries.AsNoTracking().ToListAsync();
                // Set high priority and no expiration
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetPriority(CacheItemPriority.High);
                _cache.Set(cacheKey, countries, cacheEntryOptions);
            }
            return countries ?? new List<Country>();
        }
        public void RemoveCountriesFromCache()
        {
            var cacheKey = "Countries";
            _cache.Remove(cacheKey);
        }
        public async Task AddCountry(Country country)
        {
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();
            RemoveCountriesFromCache();
        }
        public async Task UpdateCountry(Country updatedCountry)
        {
            _context.Countries.Update(updatedCountry);
            await _context.SaveChangesAsync();
            RemoveCountriesFromCache();
        }
        // States: Sliding expiration + Normal priority
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
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                    .SetPriority(CacheItemPriority.Normal);
                _cache.Set(cacheKey, states, cacheEntryOptions);
            }
            return states ?? new List<State>();
        }
        // Cities: Absolute expiration + Low priority
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
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30))
                    .SetPriority(CacheItemPriority.Low);
                _cache.Set(cacheKey, cities, cacheEntryOptions);
            }
            return cities ?? new List<City>();
        }
    }
}