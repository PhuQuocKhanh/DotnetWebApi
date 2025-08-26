using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthorizationCustomFilter.Filters
{
    // Thuộc tính ủy quyền tùy chỉnh để hạn chế truy cập API dựa trên giờ làm việc
    public class BusinessHoursAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly int _startHour;
        private readonly int _endHour;

        public BusinessHoursAuthorizeAttribute(int startHour = 9, int endHour = 18)
        {
            _startHour = startHour;
            _endHour = endHour;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = CreateJsonResponse(401, "Unauthorized", "Authentication is required to access this resource.");
                return;
            }

            var now = DateTime.Now.TimeOfDay;

            // Kiểm tra giờ hiện tại có nằm ngoài khung giờ làm việc không
            if (now.Hours < _startHour || now.Hours >= _endHour)
            {
                context.Result = CreateJsonResponse(403, "Forbidden", $"API accessible only between {_startHour}:00 and {_endHour}:00 local time.");
                return;
            }
        }

        private JsonResult CreateJsonResponse(int statusCode, string error, string message)
        {
            var jsonPayload = new { Status = statusCode, Error = error, Message = message };
            return new JsonResult(jsonPayload) { StatusCode = statusCode };
        }
    }
}