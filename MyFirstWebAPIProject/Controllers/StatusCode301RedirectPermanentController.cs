using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyFirstWebAPIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusCode301RedirectPermanentController : ControllerBase
    {
        [HttpGet("old-route")]
        public IActionResult GetFromOldRoute()
        {
            // URL của vị trí mới cho tài nguyên
            string newUrl = Url.Action("GetFromNewRoute", "StatusCode301RedirectPermanent");
            // Chuyển hướng vĩnh viễn đến URL mới với mã trạng thái HTTP 301
            return RedirectPermanent(newUrl);
        }
        
        [HttpGet("new-route")]
        public IActionResult GetFromNewRoute()
        {
            // Code xử lý yêu cầu tại vị trí mới
            return Ok("Đây là vị trí mới của tài nguyên.");
        }
    }
}