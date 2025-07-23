using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFirstWebAPIProject.Models;

namespace MyFirstWebAPIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusCode304Controller : ControllerBase
    {
        [HttpGet("{id}")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client, NoStore = false)] 
        public IActionResult Get(int id) 
        {
            var resource = GetResourceById(id);

            if (resource == null) 
            {
                return NotFound(); // 404 nếu không tìm thấy resource
            }

            var lastModified = resource.LastModified.ToUniversalTime();
            var eTag = $"{lastModified.Ticks}"; // ETag sinh từ ticks của LastModified

            // So sánh ETag client gửi lên
            if (Request.Headers.ContainsKey("If-None-Match") && Request.Headers["If-None-Match"].ToString() == eTag)
            {
                return StatusCode(StatusCodes.Status304NotModified); // Không có thay đổi, trả về 304
            }

            // So sánh thời gian client cache với thời gian LastModified
            if (Request.Headers.ContainsKey("If-Modified-Since") &&
                DateTime.TryParse(Request.Headers["If-Modified-Since"], out DateTime ifModifiedSince) &&
                ifModifiedSince >= lastModified)
            {
                return StatusCode(StatusCodes.Status304NotModified); // Không có thay đổi, trả về 304
            }

            // Set các header phản hồi
            Response.Headers["ETag"] = eTag;
            Response.Headers["Last-Modified"] = lastModified.ToString("R"); 

            return Ok(resource); // Trả về 200 kèm dữ liệu
        }

        // Giả lập truy xuất tài nguyên theo ID
        private Resource GetResourceById(int id)
        {
            return new Resource
            {
                Id = id,
                Content = "Sample Content",
                LastModified = Convert.ToDateTime("04-08-2024 04:24:37")
            };
        }
    }
}