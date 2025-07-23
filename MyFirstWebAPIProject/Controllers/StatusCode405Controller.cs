using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyFirstWebAPIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StatusCode405Controller : ControllerBase
    {
        // This action only supports POST method
        [HttpPost]
        public IActionResult PostAction()
        {
            return Ok("Data processed");
        }
        
        [HttpGet]
        public IActionResult UnifiedMethod()
        {
            if (HttpContext.Request.Method == HttpMethod.Post.Method)
            {
                return Ok("Handled POST request");
            }
            else if (HttpContext.Request.Method == HttpMethod.Put.Method)
            {
                return Ok("Handled PUT request");
            }
            else
            {
                var customResponse = new
                {
                    Code = 405,
                    Message = "Support Method are POST and PUT"
                };
                // Explicitly return 405 for Unsupported Methods
                return StatusCode(StatusCodes.Status405MethodNotAllowed, customResponse);
            }
        }
    }
}