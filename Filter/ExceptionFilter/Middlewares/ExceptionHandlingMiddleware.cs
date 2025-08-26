using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ExceptionFilter.Middlewares
{
    // Middleware tùy chỉnh để xử lý ngoại lệ một cách toàn cục
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;          // Middleware tiếp theo trong pipeline
        private readonly ILogger<ExceptionHandlingMiddleware> _logger; // Logger để ghi log lỗi
        private readonly IHostEnvironment _env;         // Để xác định môi trường (Dev/Prod)

        // Constructor với DI cho delegate tiếp theo, logger, và environment
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        // Phương thức này được gọi cho mỗi HTTP request
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Gọi middleware/component tiếp theo trong pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Bắt bất kỳ ngoại lệ nào được ném ra ở các tầng dưới
                await HandleExceptionAsync(context, ex);
            }
        }

        // Phương thức tập trung để xử lý ngoại lệ và ghi phản hồi tùy chỉnh
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Ghi log chi tiết ngoại lệ với stack trace
            _logger.LogError(exception, "Unhandled exception caught by middleware.");

            // Mặc định là 500 Internal Server Error
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var error = "Internal Server Error";
            var message = "An unexpected error occurred. Please try again later.";

            // Tùy chỉnh thông báo cho môi trường development
            if (_env.IsDevelopment())
            {
                message = exception.Message;
            }

            // Sử dụng câu lệnh switch để xử lý các loại ngoại lệ cụ thể một cách khác nhau
            switch (exception)
            {
                case ArgumentException argEx:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    error = "Bad Request";
                    message = argEx.Message;
                    break;
                case KeyNotFoundException _:
                    statusCode = (int)HttpStatusCode.NotFound;
                    error = "Not Found";
                    message = "Resource not found.";
                    break;
                case AuthenticationException _:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    error = "Unauthorized";
                    message = "Authentication failed.";
                    break;
                case UnauthorizedAccessException _:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    error = "Unauthorized";
                    message = "Unauthorized access.";
                    break;
                case InvalidOperationException _:
                    statusCode = (int)HttpStatusCode.Conflict;
                    error = "Conflict";
                    message = exception.Message;
                    break;
                case SqlException _:
                case DbUpdateException _:
                    statusCode = (int)HttpStatusCode.ServiceUnavailable;
                    error = "Database Error";
                    message = "A database error occurred. Please try again later.";
                    break;
                case NotImplementedException _:
                    statusCode = (int)HttpStatusCode.NotImplemented;
                    error = "Not Implemented";
                    message = "This functionality is not implemented.";
                    break;
                case TimeoutException _:
                    statusCode = (int)HttpStatusCode.RequestTimeout;
                    error = "Request Timeout";
                    message = "The request timed out. Please try again.";
                    break;
            }

            // Chuẩn bị payload phản hồi lỗi
            var errorResponse = new
            {
                Status = statusCode,
                Error = error,
                Message = message
            };

            // Serialize sang JSON
            var jsonResponse = JsonSerializer.Serialize(errorResponse);

            // Thiết lập content type và status code của response
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            // Ghi phản hồi lỗi JSON cho client
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}