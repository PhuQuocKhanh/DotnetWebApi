using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HTTPMethods.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourcesHeadController : ControllerBase
    {
        // Action GET: Trả về nội dung tài nguyên (để so sánh với HEAD)
        [HttpGet("GetById/{Id}")]
        public IActionResult GetById(int Id)
        {
            return Ok("Resource Returned");
        }

        // Action HEAD: Kiểm tra sự tồn tại của tài nguyên
        // URL: api/Resources/CheckResourceExists/1
        [HttpHead("CheckResourceExists/{Id}")]
        public IActionResult CheckResourceExists(int Id)
        {
            // Giả lập kiểm tra sự tồn tại từ DB
            var resourceExists = true;

            if (resourceExists)
                return Ok();           // Trả về 200 OK nếu tồn tại
            else
                return NotFound();     // Trả về 404 Not Found nếu không tồn tại
        }

        // Action HEAD: Phát hiện thay đổi của tài nguyên thông qua headers
        // URL: api/Resources/GetResourceInfo/1
        [HttpHead("GetResourceInfo/{Id}")]
        public IActionResult GetResourceHeaders(int Id)
        {
            // Giả lập metadata của tài nguyên
            var resourceMetadata = new
            {
                LastModified = DateTime.UtcNow,
                CustomHeader = "ABCDZYZ"
            };

            // Thêm các header tùy chỉnh vào response
            Response.Headers.Add("Last-Modified", resourceMetadata.LastModified.ToString("R")); // Định dạng RFC1123
            Response.Headers.Add("CustomHeader", resourceMetadata.CustomHeader);

            return Ok(); // Không có body, chỉ header
        }

        // Action HEAD: Lấy thông tin tiêu đề của tài nguyên
        // URL: api/Resources/GetResourceHeaders/1
        [HttpHead("GetResourceHeaders/{Id}")]
        public IActionResult GetResourceInfo(int Id)
        {
            // Giả lập dữ liệu tài nguyên
            var resource = new
            {
                ContentType = "application/json",
                ContentLength = 1234
            };

            if (resource == null)
                return NotFound();

            // Thiết lập các header tương ứng
            Response.Headers.Add("Content-Type", resource.ContentType);
            Response.Headers.Add("Content-Length", resource.ContentLength.ToString());

            return Ok();
        }
    }
}