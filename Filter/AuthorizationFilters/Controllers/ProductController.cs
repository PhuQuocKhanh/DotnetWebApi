using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationFilters.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        // Endpoint công khai - không yêu cầu xác thực
        [HttpGet("public")]
        [AllowAnonymous] // Cho phép bất kỳ ai (kể cả người dùng chưa xác thực) truy cập endpoint này
        public IActionResult Public()
        {
            return Ok("This is a public endpoint accessible to everyone.");
        }

        // Chỉ người dùng đã xác thực - không giới hạn vai trò
        [HttpGet("authenticated")]
        [Authorize] // Yêu cầu một JWT token hợp lệ (bất kỳ người dùng đã xác thực nào)
        public IActionResult Authenticated()
        {
            return Ok($"Hello {User.Identity?.Name}, you are authenticated.");
        }

        // Phân quyền theo vai trò duy nhất - chỉ Admin
        [HttpGet("admin-only")]
        [Authorize(Roles = "Admin")] // Chỉ người dùng có vai trò "Admin" mới có thể truy cập endpoint này
        public IActionResult AdminOnly()
        {
            return Ok("This endpoint is restricted to Admin role users only.");
        }

        // Nhiều vai trò - Logic AND (Người dùng phải có cả Manager và Admin)
        [HttpGet("manager-and-admin")]
        [Authorize(Roles = "Manager")] // Người dùng phải có vai trò "Manager"
        [Authorize(Roles = "Admin")]   // Người dùng cũng PHẢI có vai trò "Admin"
        public IActionResult ManagerAndAdmin()
        {
            return Ok("You must have BOTH Manager AND Admin roles to access this endpoint.");
        }

        // Nhiều vai trò - Logic OR (Người dùng phải có Manager hoặc User)
        [HttpGet("manager-or-user")]
        [Authorize(Roles = "Manager,User")] // Người dùng cần ít nhất một trong các vai trò này
        public IActionResult ManagerOrUser()
        {
            return Ok("You have either Manager OR User role - access granted.");
        }
    }
}