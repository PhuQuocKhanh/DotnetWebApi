using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTDemo.Data;
using JWTDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace JWTDemo.Services
{
    public interface IClientCacheService
    {
        // Phương thức bất đồng bộ: lấy client từ cache,
        // nếu không có thì lấy từ DB và cập nhật vào cache.
        Task<Client?> GetClientByClientIdAsync(string clientId);
    }

    public class ClientCacheService : IClientCacheService
    {
        // Tiền tố để định danh duy nhất mỗi client trong cache
        private const string CacheKeyPrefix = "Client_";
        // Service provider để tạo scope cho các service có lifetime là Scoped như DbContext
        private readonly IServiceProvider _serviceProvider;
        // Instance của memory cache để lưu dữ liệu client trong bộ nhớ
        private readonly IMemoryCache _memoryCache;

        // Constructor để inject các service cần thiết: IServiceProvider và IMemoryCache
        public ClientCacheService(IServiceProvider serviceProvider, IMemoryCache memoryCache)
        {
            _serviceProvider = serviceProvider;
            _memoryCache = memoryCache;
        }

        // Lấy một đối tượng Client bằng ClientId một cách bất đồng bộ.
        // Đầu tiên thử lấy từ cache.
        // Nếu không có trong cache (cache miss), sẽ lấy từ database,
        // sau đó lưu vào cache và trả về.
        public async Task<Client?> GetClientByClientIdAsync(string clientId)
        {
            // Xây dựng cache key cho client này bằng tiền tố và clientId
            var cacheKey = CacheKeyPrefix + clientId;

            // Thử lấy client từ memory cache
            if (_memoryCache.TryGetValue<Client>(cacheKey, out var client))
            {
                // Cache hit - trả về client đã cache ngay lập tức
                return client;
            }

            // Cache miss - tạo một scope mới để lấy một instance DbContext mới
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Truy vấn database bất đồng bộ để tìm client đang hoạt động khớp với clientId
            // AsNoTracking() giúp tăng hiệu năng vì EF Core không cần theo dõi thay đổi của đối tượng này
            client = await dbContext.Clients.AsNoTracking()
                .FirstOrDefaultAsync(c => c.ClientId == clientId && c.IsActive);

            if (client != null)
            {
                // Lưu client lấy được vào cache với chính sách hết hạn
                // Ở đây, cache sẽ hết hạn sau 1 giờ; có thể điều chỉnh khi cần
                _memoryCache.Set(cacheKey, client, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
            }

            // Trả về đối tượng client (hoặc null nếu không tìm thấy trong DB)
            return client;
        }
    }
}