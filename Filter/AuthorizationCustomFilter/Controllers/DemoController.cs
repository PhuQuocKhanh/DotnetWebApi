using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationCustomFilter.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationCustomFilter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DemoController : ControllerBase
    {
        // Endpoint được bảo vệ bởi bộ lọc dựa trên gói đăng ký, yêu cầu "Premium" hoặc "Pro"
        [HttpGet("premium-analytics")]
        [TypeFilter(typeof(SubscriptionBasedAuthorizationFilter), Arguments = new object[] { new[] { "Premium", "Pro" } })]
        public IActionResult GetPremiumAnalytics()
        {
            return Ok(new { message = "Welcome to Premium Analytics." });
        }

        // Endpoint bị giới hạn cho người dùng từ phòng "HR"
        [HttpGet("salary-review")]
        [TypeFilter(typeof(DepartmentAuthorizationFilter), Arguments = new object[] { "HR" })]
        public IActionResult GetSalaryReview()
        {
            return Ok(new { message = "HR department salary review data." });
        }

        // Endpoint chỉ có thể truy cập trong giờ làm việc được chỉ định (9 AM - 6 PM)
        [HttpGet("support-ticket")]
        [BusinessHoursAuthorize(9, 18)]
        public IActionResult GetSupportTicket()
        {
            return Ok(new { message = "Support ticket API (business hours only) accessed." });
        }
    }
}