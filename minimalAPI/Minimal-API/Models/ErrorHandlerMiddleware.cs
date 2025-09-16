using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Minimal_API.Models
{
    public class ErrorHandlerMiddleware
    {
        // Middleware tiếp theo trong pipeline
        private readonly RequestDelegate _next;
        // Logger để ghi log lỗi
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        // Constructor: inject middleware kế tiếp và logger
        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;     
            _logger = logger; 
        }

        // Được gọi cho mỗi HTTP request, xử lý lỗi khi có exception xảy ra
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Gọi middleware tiếp theo trong pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Ghi log exception cùng stack trace
                _logger.LogError(ex, "An unhandled exception has occurred.");

                // Set response dạng JSON
                context.Response.ContentType = "application/json";
                // Set status code 500 (Internal Server Error)
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // Tạo object chứa thông tin lỗi
                var response = new
                {
                    Title = "An unexpected error occurred.",  // Thông báo ngắn gọn, thân thiện
                    Status = context.Response.StatusCode,     // Mã lỗi HTTP (500)
                    // Hiển thị chi tiết lỗi nếu môi trường là Development
                    Detail = context.RequestServices.GetService(typeof(IWebHostEnvironment)) is IWebHostEnvironment env && env.IsDevelopment()
                             ? ex.Message                    
                             : "Please contact support."
                };

                // Serialize object lỗi sang JSON
                var jsonResponse = JsonSerializer.Serialize(response);

                // Ghi JSON response vào body trả về
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}