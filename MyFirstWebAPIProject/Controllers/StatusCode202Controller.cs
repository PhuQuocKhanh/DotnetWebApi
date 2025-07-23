using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyFirstWebAPIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StatusCode202Controller : ControllerBase
    {
        // POST: api/Job/CreateJobAsyncWithoutData
        [HttpPost]
        public async Task<IActionResult> CreateJobAsyncWithoutData()
        {
            // Start the asynchronous processing without blocking the thread.
            // The LongRunningTask will take 120 seconds to complete.
            // The main thread will not be blocked until the LongRunningTask is completed.
            await LongRunningTask();
            // Return 202 Accepted status code without any data.
            return Accepted();
        }
        // POST: api/Job/CreateJobAsyncWithData
        [HttpPost]
        public async Task<IActionResult> CreateJobAsyncWithData()
        {
            // Start the asynchronous processing without blocking the thread.
            await LongRunningTask();
            // Create a resource status object to return as part of the response.
            var resourceStatus = new
            {
                Status = "Processing",
                EstimatedCompletionTime = "2 hours"
            };
            // Return 202 Accepted status code with the resource status data.
            return Accepted(resourceStatus);
        }
        // POST: api/Job/CreateJobWithLocation
        [HttpPost]
        public async Task<IActionResult> CreateJobWithLocation()
        {
            // Start the asynchronous processing without blocking the thread.
            await LongRunningTask();
            // Assume the Processing JobId is 123.
            var processingJobId = 123;
            // Generate the dynamic Location URI to check status using the JobId.
            var locationUrl = Url.Action("GetStatus", new { JobId = processingJobId });
            // Check if the generated location URL is null or empty.
            if (string.IsNullOrEmpty(locationUrl))
            {
                // Return 400 Bad Request if the URL could not be generated.
                return BadRequest("Unable to generate status URL.");
            }
            // Create a new Uri object from the location URL.
            var locationUri = new Uri(locationUrl, UriKind.RelativeOrAbsolute);
            // Return 202 Accepted status code with a location URI.
            return Accepted(locationUri);
        }
        // POST: api/Job/CreateJobWithLocationAndData
        [HttpPost]
        public async Task<IActionResult> CreateJobWithLocationAndData()
        {
            // Start the asynchronous processing without blocking the thread.
            await LongRunningTask();
            // Assume the Processing JobId is 123.
            var processingJobId = 123;
            // Create a resource status object to return as part of the response.
            var resourceStatus = new
            {
                Status = "Processing",
                EstimatedCompletionTime = "2 hours",
                JobId = processingJobId
            };
            // Generate the dynamic Location URI to check status using the JobId.
            var locationUrl = Url.Action("GetStatus", new { JobId = processingJobId });
            // Check if the generated location URL is null or empty.
            if (string.IsNullOrEmpty(locationUrl))
            {
                // Return 400 Bad Request if the URL could not be generated.
                return BadRequest("Unable to generate status URL.");
            }
            // Create a new Uri object from the location URL.
            var locationUri = new Uri(locationUrl, UriKind.RelativeOrAbsolute);
            // Return 202 Accepted status code with a location URI and a response body containing the resource status.
            return Accepted(locationUri, resourceStatus);
        }
        // GET: api/Job/GetStatus/123
        // Define a GET endpoint named GetStatus which takes a JobId as a parameter.
        [HttpGet("{JobId}")]
        public IActionResult GetStatus(int JobId)
        {
            // Create a resource status object representing the current status of the job.
            var resourceStatus = new { Status = "Processing", EstimatedCompletionTime = "2 hours" };
            // Return 200 OK status code with the resource status data.
            return Ok(resourceStatus);
        }
        // This is the method that performs the time-consuming asynchronous operation.
        private async Task LongRunningTask()
        {
            // Simulate a long-running task with a 120 seconds delay.
            await Task.Delay(TimeSpan.FromSeconds(120));
            // Task logic goes here (not implemented in this example).
        }
    }
}