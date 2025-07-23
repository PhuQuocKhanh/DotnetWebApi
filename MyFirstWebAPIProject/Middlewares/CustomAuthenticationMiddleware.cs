using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstWebAPIProject.Middlewares
{
    public class CustomAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        // Constructor để khởi tạo middleware với middleware tiếp theo
        public CustomAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next; // Gán middleware tiếp theo vào trường _next
        }
        
        // Phương thức được gọi cho mỗi request để xử lý xác thực
        public async Task InvokeAsync(HttpContext context)
        {
            // Logic xác thực tùy chỉnh
            bool isAuthorized = CheckAuthorization(context); // Gọi phương thức kiểm tra xác thực

            if (!isAuthorized) // Nếu không được xác thực
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized; // Gán mã trạng thái HTTP 401
                context.Response.ContentType = "application/json"; // Đặt kiểu nội dung phản hồi là JSON

                // Tạo đối tượng phản hồi tùy chỉnh
                var customResponse = new
                {
                    status = 401, // Mã trạng thái
                    message = "Unauthorized. Please Provide Valid Credentials" // Thông báo lỗi tùy chỉnh
                };

                // Tuần tự hóa đối tượng phản hồi thành JSON và ghi vào body phản hồi
                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(customResponse));
                return; // Ngắt pipeline, không cho các middleware tiếp theo thực thi
            }

            // Nếu được xác thực, chuyển tiếp request đến middleware tiếp theo trong pipeline
            await _next(context);
        }

        // Phương thức kiểm tra xác thực (private)
        private bool CheckAuthorization(HttpContext context)
        {
            // Cài đặt logic xác thực ở đây
            // Ví dụ: kiểm tra Header hoặc Token cụ thể
            // Mô phỏng không xác thực trong ví dụ này
            return false; // Luôn trả về false để mô phỏng request không được xác thực
        }
    }
}