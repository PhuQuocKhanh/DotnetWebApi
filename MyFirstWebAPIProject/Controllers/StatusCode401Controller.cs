using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyFirstWebAPIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StatusCode401Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult SecureResource()
        {
            // Your authentication logic here
            // Assuming this is the result of your auth check
            bool isAuthenticated = false;
            if (!isAuthenticated)
            {
                return Unauthorized(); // Returns a 401 Unauthorized response
            }
            // Proceed with normal action if authenticated
            return Ok("Authenticated and Authorized Access.");
        }

        [HttpGet]
        public IActionResult ManualReturn()
        {
            // Your authentication logic here
            // Assuming this is the result of your auth check
            bool isAuthenticated = false;
            if (!isAuthenticated)
            {
                // Returns a 401 Unauthorized response
                var errorResponse = new
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "Access denied. Please provide valid credentials."
                };
                // Use StatusCode method to return 401 Unauthorized status and custom data
                return StatusCode(StatusCodes.Status401Unauthorized, errorResponse);
            }
            // Proceed with normal action if authenticated
            return Ok("Authenticated and Authorized Access.");
        }
    }
}