using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ExceptionFilter.Filters
{
    // Custom Exception Filter triển khai giao diện IExceptionFilter
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger; // Logger để ghi log các ngoại lệ
        private readonly IHostEnvironment _env;                  // Cung cấp thông tin về môi trường hosting

        // Constructor nhận ILogger và IHostEnvironment thông qua Dependency Injection
        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger, IHostEnvironment env)
        {
            _logger = logger;  // Gán instance logger cho trường private
            _env = env;        // Gán instance environment cho trường private
        }

        // Phương thức này được gọi khi một ngoại lệ chưa được xử lý xảy ra trong pipeline request
        public void OnException(ExceptionContext context)
        {
            // Ghi log chi tiết ngoại lệ với stack trace và message ở mức độ Error
            _logger.LogError(context.Exception, "Unhandled exception occurred.");

            // Khởi tạo mã trạng thái HTTP mặc định là 500 Internal Server Error
            var statusCode = (int)HttpStatusCode.InternalServerError;
            // Khởi tạo tiêu đề lỗi mặc định
            var error = "Internal Server Error";
            // Khởi tạo thông báo lỗi thân thiện với client mặc định
            var message = "An unexpected error occurred. Please try again later.";

            // Nếu ứng dụng đang chạy trong môi trường Development
            if (_env.IsDevelopment())
            {
                // Cung cấp thông báo ngoại lệ chi tiết để dễ dàng gỡ lỗi
                message = context.Exception.Message;
            }

            // Sử dụng câu lệnh switch để xử lý các loại ngoại lệ cụ thể một cách khác nhau
            switch (context.Exception)
            {
                // Xử lý lỗi xác thực tham số (ví dụ: tham số phương thức không hợp lệ)
                case ArgumentException argEx:
                    statusCode = (int)HttpStatusCode.BadRequest;  // 400 Bad Request
                    error = "Bad Request";
                    message = argEx.Message;                       // Sử dụng thông báo của ngoại lệ
                    break;
                // Xử lý lỗi không tìm thấy tài nguyên
                case KeyNotFoundException _:
                    statusCode = (int)HttpStatusCode.NotFound;    // 404 Not Found
                    error = "Not Found";
                    message = "Resource not found.";               // Thông báo chung cho client
                    break;
                // Xử lý lỗi xác thực không thành công
                case AuthenticationException _:
                    statusCode = (int)HttpStatusCode.Unauthorized; // 401 Unauthorized
                    error = "Unauthorized";
                    message = "Authentication failed.";
                    break;
                // Xử lý các nỗ lực truy cập trái phép
                case UnauthorizedAccessException _:
                    statusCode = (int)HttpStatusCode.Unauthorized; // 401 Unauthorized
                    error = "Unauthorized";
                    message = "Unauthorized access.";
                    break;
                // Xử lý các hoạt động không hợp lệ (ví dụ: xung đột trạng thái)
                case InvalidOperationException _:
                    statusCode = (int)HttpStatusCode.Conflict;     // 409 Conflict
                    error = "Conflict";
                    message = context.Exception.Message;           // Sử dụng thông báo chi tiết
                    break;
                // Xử lý ngoại lệ SQL Server (ví dụ: lỗi kết nối)
                case SqlException _:
                // Xử lý ngoại lệ cập nhật của Entity Framework (ví dụ: vấn đề cập nhật cơ sở dữ liệu)
                case DbUpdateException _:
                    statusCode = (int)HttpStatusCode.ServiceUnavailable; // 503 Service Unavailable
                    error = "Database Error";
                    message = "A database error occurred. Please try again later.";
                    break;
                // Xử lý ngoại lệ chức năng chưa được triển khai
                case NotImplementedException _:
                    statusCode = (int)HttpStatusCode.NotImplemented;    // 501 Not Implemented
                    error = "Not Implemented";
                    message = "This functionality is not implemented.";
                    break;
                // Xử lý request hết thời gian chờ
                case TimeoutException _:
                    statusCode = (int)HttpStatusCode.RequestTimeout;    // 408 Request Timeout
                    error = "Request Timeout";
                    message = "The request timed out. Please try again.";
                    break;
                // Bạn có thể thêm các case khác ở đây để xử lý các loại ngoại lệ khác khi cần
            }

            // Tạo một đối tượng vô danh để cấu trúc phản hồi lỗi JSON
            var errorResponse = new
            {
                Status = statusCode, // Mã trạng thái HTTP (ví dụ: 404, 500)
                Error = error,       // Tiêu đề lỗi ngắn gọn
                Message = message    // Thông báo lỗi chi tiết cho client
            };

            // Gán phản hồi JSON đã xây dựng làm kết quả HTTP
            context.Result = new JsonResult(errorResponse)
            {
                StatusCode = statusCode // Đặt mã trạng thái HTTP thích hợp trên phản hồi
            };

            // Đánh dấu ngoại lệ đã được xử lý để ASP.NET Core không ném lại nó
            context.ExceptionHandled = true;
        }
    }
}