using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyFirstWebAPIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StatusCode202AcceptedAtActionController : ControllerBase
    {
        // POST: api/Job/CreateJobWithLocation
        [HttpPost]
        public async Task<IActionResult> CreateJobWithLocation()
        {
            // Start the asynchronous processing without blocking the thread.
            LongRunningTask();
            // Assume the Processing JobId is 123.
            var processingJobId = 123;
            // Return 202 Accepted status code with a location URI pointing to the GetStatus action.
            // Assuming the job has an ID assigned after being added
            // Passing object value as null as we don't want to return any data
            // Here, GetStatus is the Action Method Name
            return AcceptedAtAction("GetStatus", new { JobId = processingJobId }, null);
        }
        // POST: api/Job/CreateJobWithLocationAndData
        [HttpPost]
        public async Task<IActionResult> CreateJobWithLocationAndData()
        {
            // Start the asynchronous processing without blocking the thread.
            LongRunningTask();
            // Assume the Processing JobId is 123.
            var processingJobId = 123;
            // Create a resource status object to return as part of the response.
            var resourceStatus = new
            {
                Status = "Processing",
                EstimatedCompletionTime = "2 hours",
                JobId = processingJobId
            };
            // Return 202 Accepted status code with a location URI and the resource status data.
            // Here, GetStatus is the Action Method Name and Job is the Controller Name
            // return AcceptedAtAction("GetStatus", new { JobId = processingJobId }, resourceStatus);
            return AcceptedAtAction("GetStatus", "Job", new { JobId = processingJobId }, resourceStatus);
        }
        // GET: api/Job/GetStatus/123
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