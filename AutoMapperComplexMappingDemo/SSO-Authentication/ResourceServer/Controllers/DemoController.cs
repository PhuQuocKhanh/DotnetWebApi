using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ResourceServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DemoController : ControllerBase
    {
        // Public Endpoint
        [HttpGet("public-data")]
        public IActionResult GetPublicData()
        {
            var publicData = new
            {
                Message = "This is public data accessible without authentication."
            };
            return Ok(publicData);
        }

        // Protected Endpoint
        [Authorize]
        [HttpGet("protected-data")]
        public IActionResult GetProtectedData()
        {
            var protectedData = new
            {
                Message = "This is protected data accessible only with a valid JWT token."
            };
            return Ok(protectedData);
        }
    }
}