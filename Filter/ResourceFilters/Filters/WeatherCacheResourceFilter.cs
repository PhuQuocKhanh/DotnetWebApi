using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace ResourceFilters.Filters
{
    // Lớp này triển khai một resource filter để cache response dữ liệu thời tiết cho mỗi thành phố
    public class WeatherCacheResourceFilter : IResourceFilter
    {
        // Instance IMemoryCache được tiêm vào để lưu trữ và truy xuất dữ liệu cache
        private readonly IMemoryCache _memoryCache;
        // Định nghĩa thời gian dữ liệu cache sẽ được lưu trữ (ở đây là 60 giây)
        private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(60);

        // Constructor nhận IMemoryCache thông qua dependency injection
        public WeatherCacheResourceFilter(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        // Phương thức này chạy trước khi action method được thực thi
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            // Trích xuất tham số route 'city' từ URL request, chuyển thành chữ thường để nhất quán
            var city = context.RouteData.Values["city"]?.ToString()?.ToLower();

            // Nếu city bị thiếu hoặc rỗng, không làm gì và để request tiếp tục
            if (string.IsNullOrEmpty(city))
                return; // Thoát nếu city bị thiếu

            // Nếu bạn dùng query string cho city
            // var city = context.HttpContext.Request.Query["city"].ToString()?.ToLower();

            // Cố gắng truy xuất dữ liệu cache cho thành phố này từ memory cache
            // 'cachedData' sẽ chứa dữ liệu thời tiết đã cache nếu nó tồn tại
            if (_memoryCache.TryGetValue(city, out object? cachedData) && cachedData != null)
            {
                // Nếu tìm thấy dữ liệu cache, ngắt mạch pipeline request ngay lập tức
                // bằng cách trả về dữ liệu cache dưới dạng JSON với mã trạng thái HTTP 200 OK
                context.Result = new JsonResult(cachedData)
                {
                    StatusCode = 200
                };
            }
            // Nếu không tìm thấy dữ liệu cache, request sẽ tiếp tục đến action method một cách bình thường
        }

        // Phương thức này chạy sau khi action method đã được thực thi
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            // Trích xuất lại tham số route 'city' để cache response
            var city = context.RouteData.Values["city"]?.ToString()?.ToLower();

            // Nếu bạn dùng query string cho city
            // var city = context.HttpContext.Request.Query["city"].ToString()?.ToLower();

            // Nếu city bị thiếu hoặc rỗng, không làm gì (không thể cache nếu không có key)
            if (string.IsNullOrEmpty(city))
                return; // Thoát nếu city bị thiếu

            // Kiểm tra xem action method có trả về kết quả 200 OK thành công với giá trị không null hay không
            if (context.Result is ObjectResult result && result.StatusCode == 200 && result.Value is not null)
            {
                // Lưu trữ giá trị kết quả vào memory cache với city làm khóa
                // Thời gian cache được đặt là CacheDuration (60 giây)
                _memoryCache.Set(city, result.Value, CacheDuration);
            }
            // Nếu kết quả không thành công hoặc giá trị là null, không cache gì cả
        }
    }
}