using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ResourceFilters.Filters
{
 // Attribute tùy chỉnh triển khai một resource filter đồng bộ để xác thực sự hiện diện của API key
    // Có thể được áp dụng cho các controller hoặc action như một attribute
    public class ApiKeyValidationResourceFilterAttribute : Attribute, IResourceFilter
    {
        // Tên của header HTTP dự kiến chứa API key
        private const string ApiKeyHeaderName = "X-API-KEY";

        // Phương thức này chạy trước khi action method được thực thi
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            // Truy cập bộ sưu tập các header của request HTTP
            var headers = context.HttpContext.Request.Headers;

            // Thử lấy giá trị của header API key
            // Nếu header bị thiếu hoặc rỗng, extractedApiKey sẽ là null hoặc rỗng
            if (!headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                // Chuẩn bị một payload JSON chỉ ra truy cập không được phép do thiếu API key
                var payload = new
                {
                    Status = 401,
                    Message = "Unauthorized: API Key is missing."
                };

                // Gán ngay lập tức response HTTP thành 401 Unauthorized với payload
                // Điều này ngắt mạch pipeline request và ngăn action thực thi
                context.Result = new JsonResult(payload)
                {
                    StatusCode = 401
                };

                // return để dừng xử lý tiếp theo của request
                return;
            }
            // Nếu header API key có mặt, không có hành động nào được thực hiện ở đây và request tiếp tục
        }

        // Phương thức này chạy sau khi action method được thực thi
        // Không cần hậu xử lý trong bộ lọc này sau khi thực thi action
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            // Không cần triển khai gì ở đây
        }
    }
}