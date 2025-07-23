using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyFirstWebAPIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusCode302Controller : ControllerBase
    {
        [HttpGet]
        [Route("OldEndpoint")]
        public IActionResult GetFromOldEndpoint()
        {
            // The URL of the new location for the resource
            //string newUrl = "https://localhost:7128//api/Employee/NewEndpoint";
            //return Redirect("/api/Employee/NewEndpoint");
            string? newUrl = Url.Action("GetFromNewEndpoint", "StatusCode302");
            if (newUrl == null)
            {
                // Handle the error or generate a default URI
                return BadRequest("Unable to generate location URI for the new resource.");
            }
            // Temporary redirect to the new endpoint
            return Redirect(newUrl);
        }
        [HttpGet]
        [Route("NewEndpoint")]
        public IActionResult GetFromNewEndpoint()
        {
            // Handle the request as usual
            return Ok("This is the new endpoint.");
        }
    }
}