using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ActionFilter.Filters
{
    // Action filter tùy chỉnh triển khai interface đồng bộ IActionFilter
    public class RequestResponseLoggingFilter : IActionFilter
    {
        // Instance ILogger để ghi log thông tin, được inject qua constructor
        private readonly ILogger<RequestResponseLoggingFilter> _logger;
        // Constructor nhận ILogger<RequestResponseLoggingFilter> qua Dependency Injection
        public RequestResponseLoggingFilter(ILogger<RequestResponseLoggingFilter> logger)
        {
            _logger = logger; // Gán instance logger vào trường private
        }
        // Phương thức này thực thi ngay trước khi action method chạy
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Lấy HttpContext từ context của action hiện tại
            var httpContext = context.HttpContext;
            // Lấy request HTTP đến từ HttpContext
            var request = httpContext.Request;
            // Serialize các tham số query string thành chuỗi JSON để ghi log
            var query = JsonSerializer.Serialize(request.Query);
            // Serialize route data (như các tham số trên URL) thành chuỗi JSON để ghi log
            var routeData = JsonSerializer.Serialize(context.RouteData.Values);
            // Serialize tất cả các header của request HTTP thành chuỗi JSON để ghi log
            var headers = JsonSerializer.Serialize(request.Headers);
            // Ghi log thông tin chi tiết về request HTTP đến
            // Bao gồm HTTP method (GET, POST, v.v.), đường dẫn, tham số query, route data, và headers
            _logger.LogInformation($"Request Incoming: Method={request.Method}, Path={request.Path}, Query={query}, RouteData={routeData}, Headers={headers}");
        }
        // Phương thức này thực thi ngay sau khi action method đã chạy xong
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Lấy HttpContext từ context của action hiện tại
            var httpContext = context.HttpContext;
            // Lấy response HTTP sẽ được gửi đến client
            var response = httpContext.Response;
            // Ghi log HTTP status code của response đi (ví dụ: 200, 404, 500)
            _logger.LogInformation($"Response Outgoing: StatusCode={response.StatusCode}");
        }
    }
}