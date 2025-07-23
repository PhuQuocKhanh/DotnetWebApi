using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyFirstWebAPIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StatusCode500Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult ThrowException500()
        {
            try
            {
                // Your logic here
                int x = 10, y = 0;
                int z = x / y; //This statement will throw exception
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception details
                
                var customResponse = new
                {
                    Code = 500,
                    Message = "Internal Server Error",
                    // Do not expose the actual error to the client
                    ErrorMessage = ex.Message
                };
                return StatusCode(StatusCodes.Status500InternalServerError, customResponse);
            }
        }
    }
}