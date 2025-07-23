using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyFirstWebAPIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StatusCode404Controller : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetResourceNotFound(int id)
        {
            var resource = FindResourceById(id);
            if (resource == null)
            {
                // Không tìm thấy tài nguyên, trả về 404
                return NotFound();
            }
            // Tìm thấy tài nguyên, trả về với mã trạng thái 200 OK
            return Ok(resource);
        }

        [HttpGet("{id}")]
        public IActionResult GetResourceCustomErrorMessage(int id)
        {
            var resource = FindResourceById(id);
            if (resource == null)
            {
                // Resource not found, return 404
                var customResponse = new { message = $"No Employee Found with the Id: {id}" };
                return NotFound(customResponse);
            }
            // Resource found, return it with 200 OK status
            return Ok(resource);
        }

        [HttpGet("{id}")]
        public IActionResult GetResourceManualReturn(int id)
        {
            var resource = FindResourceById(id);
            if (resource == null)
            {  
                // Resource not found, return 404
                var customResponse = new { message = $"No Employee Found with the Id: {id}" };
                return StatusCode(StatusCodes.Status404NotFound, customResponse);
            }
            // Resource found, return it with 200 OK status
            return Ok(resource);
        }

        // Phương thức giả lập để mô phỏng việc tìm tài nguyên
        private object FindResourceById(int id)
        {
            // Giả định phương thức này trả về null nếu không tìm thấy tài nguyên
            // Trong ứng dụng thực tế, bạn sẽ truy vấn từ database hoặc nguồn dữ liệu tại đây
            return null;
        }
    }
}