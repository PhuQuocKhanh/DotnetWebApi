using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ResultFilter.Filter
{
    public class AuditLoggingResultFilter : ResultFilterAttribute
    {
        private readonly ILogger<AuditLoggingResultFilter> _logger;

        // Constructor với việc tiêm phụ thuộc ILogger để ghi log
        public AuditLoggingResultFilter(ILogger<AuditLoggingResultFilter> logger)
        {
            _logger = logger;
        }

        // Phương thức này chạy sau khi action result đã được thực thi và response đã sẵn sàng
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            // Lấy HTTP context hiện tại (chứa thông tin request và response)
            var httpContext = context.HttpContext;
            // Lấy đối tượng HTTP response để lấy mã trạng thái, header, v.v.
            var response = httpContext.Response;
            // Lấy principal người dùng đã được xác thực liên quan đến request
            var user = httpContext.User;

            // Cố gắng trích xuất ID của người dùng từ các claim của họ (ClaimTypes.NameIdentifier)
            // Nếu không tìm thấy (chưa xác thực), sẽ dùng "Anonymous"
            string userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";

            // Lấy kích thước gần đúng của payload response bằng byte nếu có
            long? payloadSize = response.ContentLength;

            // Ghi log chi tiết kiểm toán: mã trạng thái, ID người dùng, kích thước payload, đường dẫn request, và dấu thời gian (UTC)
            _logger.LogInformation("Audit Log - Response Sent: " +
                                   $"StatusCode={response.StatusCode}, UserId={userId}, PayloadSize={payloadSize} bytes, " +
                                   $"Path={httpContext.Request.Path}, Timestamp={DateTime.UtcNow}");

            // Gọi triển khai của lớp cơ sở (nếu có hành vi bổ sung được định nghĩa)
            base.OnResultExecuted(context);
        }
    }
}