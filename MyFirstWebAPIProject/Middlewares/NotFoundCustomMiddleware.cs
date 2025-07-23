using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyFirstWebAPIProject.Middlewares
{
    public class NotFoundCustomMiddleware
    {
        // Delegate trỏ đến middleware kế tiếp trong pipeline
        private readonly RequestDelegate _next;

        // Constructor nhận middleware kế tiếp
        public NotFoundCustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Phương thức xử lý context HTTP
        public async Task Invoke(HttpContext context)
        {
            // Gọi middleware kế tiếp
            await _next(context);

            // Kiểm tra nếu mã trạng thái là 404 và response chưa bị ghi
            if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
            {
                // Đặt kiểu nội dung là JSON
                context.Response.ContentType = "application/json";

                // Tạo phản hồi JSON tùy chỉnh
                var customResponse = new
                {
                    Code = 404,
                    Message = "Endpoint does not exist"
                };

                // Chuyển đối tượng thành chuỗi JSON
                var responseJson = JsonSerializer.Serialize(customResponse);

                // Ghi phản hồi JSON vào output
                await context.Response.WriteAsync(responseJson);
            }
        }
    }
}