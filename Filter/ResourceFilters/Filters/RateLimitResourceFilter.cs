using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace ResourceFilters.Filters
{
    public class RateLimitResourceFilter
    {
         // Instance IMemoryCache được tiêm vào để theo dõi số lượng request cho mỗi IP client
        private readonly IMemoryCache _cache;
        // Số request tối đa được phép mỗi phút cho mỗi IP client
        private readonly int _maxRequestsPerMinute = 5;

        // Constructor nhận IMemoryCache qua dependency injection
        public RateLimitResourceFilter(IMemoryCache cache)
        {
            _cache = cache;
        }

        // Phương thức bất đồng bộ này chạy trước khi action method được thực thi
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            // Lấy địa chỉ IP của client dưới dạng chuỗi; mặc định là "unknown" nếu không tìm thấy
            var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            // Tạo một khóa cache duy nhất cho mỗi IP để theo dõi số lượng request
            var cacheKey = $"RateLimit_{ip}";

            // Thử lấy số lượng request hiện tại cho IP này từ cache; mặc định là 0 nếu không có
            var requestCount = _cache.Get<int?>(cacheKey) ?? 0;

            // Kiểm tra xem client đã đạt hoặc vượt quá giới hạn request chưa
            if (requestCount >= _maxRequestsPerMinute)
            {
                // Xây dựng một response lỗi JSON báo rằng có quá nhiều request
                var errorResponse = new
                {
                    Status = 429,
                    Message = "Too Many Requests. Please try again later."
                };

                // Gán ngay lập tức response HTTP với trạng thái 429 và thông báo lỗi
                context.Result = new JsonResult(errorResponse)
                {
                    StatusCode = 429
                };

                // Ngắt mạch pipeline request; không thực thi action hay các bộ lọc khác
                return;
            }
            else
            {
                // Tăng số lượng request và lưu lại vào cache với thời gian hết hạn là 1 phút
                _cache.Set(cacheKey, requestCount + 1, TimeSpan.FromMinutes(1));
            }

            // Nếu giới hạn tần suất chưa bị vượt quá, tiếp tục thực thi pipeline request
            await next();
        }
    }
}