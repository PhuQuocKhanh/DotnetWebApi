using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ModelBinding.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserFromHeaderController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUser([FromHeader] string authToken)
        {
            // The value of the authToken parameter will be set from the "authToken" header in the request
            return Ok(authToken);
        }
        [HttpGet]
        public IActionResult GetResource([FromHeader] string Authorization)
        {
            // Implementation
            if (Authorization == null)
            {
                return BadRequest("Authorization Token is Missing");
            }
            return Ok("Request Processed Successfully");
        }

        [HttpGet]
        public IActionResult GetResourceCustomize([FromHeader(Name = "Custom-Header")] string customHeader)
        {
            if(customHeader == null)
            {
                return BadRequest("Custom-Header is Missing");
            }
            return Ok("Request Processed Successfully");
        }
    }
}