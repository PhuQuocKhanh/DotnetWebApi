using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MyFirstWebAPIProject.Controllers
{
    [Route("[controller]/[action]")]
    public class ConfigureHTTPAllowController : ControllerBase
    {
        // This action only supports GET method
        [HttpGet]
        public IActionResult GetAction()
        {
            return Ok("GetAction Data processed");
        }
        // This action only supports POST method
        [HttpPost]
        public IActionResult PostAction()
        {
            return Ok("PostAction Data Processed");
        }
        // This action only supports PUT method
        [HttpPut]
        public IActionResult PutAction()
        {
            return Ok("PutAction Data processed");
        }
        // This action only supports PUT method
        [HttpPatch]
        public IActionResult PatchAction()
        {
            return Ok("PatchAction Data processed");
        }
        // This action only supports DELETE method
        [HttpDelete]
        public IActionResult DeleteAction()
        {
            return Ok("DeleteAction Data processed");
        }

    }
}