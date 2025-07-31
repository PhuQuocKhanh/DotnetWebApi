using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HTTPMethods.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourcesOptionsController : ControllerBase
    {
         // Ví dụ phương thức GET
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("GET Response");
        }

        // Xử lý yêu cầu OPTIONS
        [HttpOptions]
        public IActionResult Options()
        {
            Response.Headers.Add("Allow", "GET, POST, OPTIONS");
            return Ok();
        }
    }
}