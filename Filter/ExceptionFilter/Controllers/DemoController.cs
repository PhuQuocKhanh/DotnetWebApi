using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ExceptionFilter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DemoController : ControllerBase
    {
        // Phương thức HTTP GET cho route: api/demo/argument
        [HttpGet("argument")]
        public IActionResult ThrowArgumentException()
        {
            // Cố ý ném một ArgumentException với một thông báo cụ thể
            // Điều này mô phỏng một kịch bản mà người dùng cung cấp đầu vào không hợp lệ
            // Nếu bạn đã đăng ký một exception filter, nó sẽ bắt lỗi này và trả về phản hồi 400 Bad Request
            throw new ArgumentException("Invalid parameter value provided.");
        }

        // Phương thức HTTP GET cho route: api/demo/notfound
        [HttpGet("notfound")]
        public IActionResult ThrowNotFoundException()
        {
            // Cố ý ném một KeyNotFoundException với một thông báo tùy chỉnh
            // Điều này mô phỏng một kịch bản mà tài nguyên được yêu cầu không tồn tại
            // Exception filters có thể bắt lỗi này và trả về phản hồi 404 Not Found với một thông báo lỗi tùy chỉnh
            throw new KeyNotFoundException("The requested item was not found.");
        }

        // Phương thức HTTP GET cho route: api/demo/unexpected
        [HttpGet("unexpected")]
        public IActionResult ThrowUnexpectedException()
        {
            // Ném một Exception chung để mô phỏng một lỗi máy chủ không được xử lý
            // Nếu không có xử lý tùy chỉnh, điều này sẽ dẫn đến phản hồi 500 Internal Server Error
            // Exception filters có thể ghi log lỗi này và trả về một phản hồi lỗi thân thiện
            throw new Exception("Simulated unhandled server error.");
        }

        // Phương thức HTTP GET cho route: api/demo/ok
        [HttpGet("ok")]
        public IActionResult GetOk()
        {
            // Trả về một phản hồi HTTP 200 OK với một payload JSON
            // Cho biết endpoint đã thực thi thành công mà không có ngoại lệ nào
            return Ok(new { Message = "No exception, everything is OK!" });
        }
    }
}