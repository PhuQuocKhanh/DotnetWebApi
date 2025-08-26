using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthorizationCustomFilter.Filters
{
     // Bộ lọc ủy quyền bất đồng bộ tùy chỉnh hạn chế truy cập dựa trên claim phòng ban
    public class DepartmentAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly string _allowedDepartment;

        public DepartmentAuthorizationFilter(string allowedDepartment)
        {
            _allowedDepartment = allowedDepartment;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = CreateJsonResponse(401, "Unauthorized", "Authentication is required to access this resource.");
                return;
            }

            var department = user.FindFirst("Department")?.Value;

            // Kiểm tra claim phòng ban không tồn tại hoặc không khớp
            if (department == null || !string.Equals(department, _allowedDepartment, StringComparison.OrdinalIgnoreCase))
            {
                context.Result = CreateJsonResponse(403, "Forbidden", $"Access restricted to {_allowedDepartment} department only.");
                return;
            }

            // Hoàn thành task vì đây là phương thức async
            await Task.CompletedTask;
        }

        private JsonResult CreateJsonResponse(int statusCode, string error, string message)
        {
            var jsonPayload = new { Status = statusCode, Error = error, Message = message };
            return new JsonResult(jsonPayload) { StatusCode = statusCode };
        }
    }
}