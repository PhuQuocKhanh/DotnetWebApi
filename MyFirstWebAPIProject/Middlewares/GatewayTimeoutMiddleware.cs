using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyFirstWebAPIProject.Middlewares
{
    public class GatewayTimeoutMiddleware
    {
        private readonly RequestDelegate _next; // Middleware tiếp theo trong pipeline

        public GatewayTimeoutMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Tiếp tục thực hiện request tới middleware kế tiếp
                await _next(context);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
            {
                // Bắt lỗi HttpRequestException khi status code là 504 Gateway Timeout
                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = StatusCodes.Status504GatewayTimeout;
                    context.Response.ContentType = "application/json";

                    var customResponse = new
                    {
                        Code = 504,
                        Message = "Máy chủ không nhận được phản hồi kịp thời từ máy chủ đầu nguồn."
                    };

                    var responseJson = JsonSerializer.Serialize(customResponse);
                    await context.Response.WriteAsync(responseJson);
                }
            }
            catch (Exception ex)
            {
                // Bắt tất cả các lỗi khác (ví dụ: lỗi hệ thống)
                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    var customResponse = new
                    {
                        Code = 500,
                        Message = "Lỗi máy chủ nội bộ",
                        Details = ex.Message
                    };

                    var responseJson = JsonSerializer.Serialize(customResponse);
                    await context.Response.WriteAsync(responseJson);
                }
            }
        }
    }
}