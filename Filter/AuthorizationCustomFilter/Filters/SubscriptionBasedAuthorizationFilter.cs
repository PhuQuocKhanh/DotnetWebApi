using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthorizationCustomFilter.Filters
{
    public class SubscriptionBasedAuthorizationFilter : IAuthorizationFilter
    {
        private readonly string[] _allowedSubscriptions;

        public SubscriptionBasedAuthorizationFilter(params string[] allowedSubscriptions)
        {
            _allowedSubscriptions = allowedSubscriptions;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            // Kiểm tra xem người dùng đã xác thực chưa; nếu chưa, trả về 401 Unauthorized
            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = CreateJsonResponse(401, "Unauthorized", "Authentication is required to access this resource.");
                return; // Dừng xử lý
            }

            var subscriptionLevel = user.FindFirst("SubscriptionLevel")?.Value;
            var subExpires = user.FindFirst("SubscriptionExpiresOn")?.Value;

            // Kiểm tra cấp độ gói đăng ký của người dùng KHÔNG nằm trong danh sách cho phép
            if (!_allowedSubscriptions.Contains(subscriptionLevel))
            {
                // Trả về 403 Forbidden
                context.Result = CreateJsonResponse(403, "Forbidden", "Your subscription level does not allow access to this resource.");
                return;
            }

            // Nếu claim hết hạn tồn tại và gói đăng ký đã hết hạn
            if (subExpires != null && DateTime.TryParse(subExpires, out var exp) && exp < DateTime.UtcNow)
            {
                // Trả về 401 Unauthorized với thông báo hết hạn
                context.Result = CreateJsonResponse(401, "Unauthorized", "Your subscription has expired.");
                return;
            }
        }

        // Phương thức trợ giúp để tạo phản hồi JSON chuẩn hóa
        private JsonResult CreateJsonResponse(int statusCode, string error, string message)
        {
            var jsonPayload = new { Status = statusCode, Error = error, Message = message };
            return new JsonResult(jsonPayload) { StatusCode = statusCode };
        }
    }
}