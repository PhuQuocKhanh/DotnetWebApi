using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyFirstWebAPIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StatusCode403Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult WithoutData()
        {
            // Kiểm tra quyền truy cập của người dùng
            bool hasPermission = CheckUserPermission();
            if (!hasPermission)
            {
                // Người dùng không có quyền truy cập
                // Trả về mã trạng thái 403 (Forbidden)
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            // Người dùng có quyền => trả về tài nguyên
            return Ok("Nội dung tài nguyên ở đây");
        }

        [HttpGet]
        public IActionResult GetResourceWithData()
        {
            // Kiểm tra quyền truy cập của người dùng
            bool hasPermission = CheckUserPermission();
            if (!hasPermission)
            {
                // Tạo đối tượng phản hồi lỗi
                var errorResponse = new 
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                    Message = "Truy cập bị từ chối. Bạn không có quyền truy cập tài nguyên này."
                };

                // Trả về mã trạng thái 403 kèm dữ liệu lỗi
                return StatusCode(StatusCodes.Status403Forbidden, errorResponse);
            }

            // Nếu có quyền, trả về tài nguyên
            return Ok("Nội dung tài nguyên ở đây");
        }

        [HttpGet]
        public IActionResult GetResourceWithMiddleware()
        {
            return Ok("Resource Content Here");
        }

        private bool CheckUserPermission()
        {
            // Giả lập logic kiểm tra quyền người dùng
            return false;
        }
    }
}