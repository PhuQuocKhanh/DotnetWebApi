using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyFirstWebAPIProject.Middlewares
{
    public class MaintenanceMiddleware503
    {
        private readonly RequestDelegate _next;

        // Constructor nhận middleware kế tiếp trong pipeline
        public MaintenanceMiddleware503(RequestDelegate next)
        {
            _next = next;
        }

        // Phương thức chính được gọi cho mỗi HTTP request
        public async Task Invoke(HttpContext httpContext, IConfiguration configuration)
        {
            // Đọc giá trị từ cấu hình để kiểm tra chế độ bảo trì
            bool isUnderMaintenance = Convert.ToBoolean(configuration["IsApplicationUnderMaintenance"]);

            if (isUnderMaintenance)
            {
                // Trả về mã HTTP 503 (Service Unavailable)
                httpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                httpContext.Response.ContentType = "application/json";

                // Header thông báo client nên thử lại sau 120 giây
                httpContext.Response.Headers["Retry-After"] = "120";

                // Tạo đối tượng phản hồi
                var customResponse = new
                {
                    Code = 503,
                    Message = "Service is under maintenance. Please try again later."
                };

                // Serialize sang JSON và ghi vào response body
                var responseJson = JsonSerializer.Serialize(customResponse);
                await httpContext.Response.WriteAsync(responseJson);
            }
            else
            {
                // Nếu không bảo trì thì chuyển tiếp request đến middleware tiếp theo
                await _next(httpContext);
            }
        }
    }
}