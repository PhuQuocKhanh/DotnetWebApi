using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyFirstWebAPIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StatusCode202AcceptedAtRouteResultController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateJobWithLocation()
        {
            // Logic to start asynchronous processing and we are not blocking the thread
            LongRunningTask();
            //Let is asume the Processing JobId is 123
            var processingJobId = 123;
            // Assuming the job has an ID assigned after being added
            // GetJobStatus is the Route Name
            return AcceptedAtRoute("GetJobStatus", new { JobId = processingJobId });
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
            return AcceptedAtRoute(new { controller = "Job", action = "GetStatus", JobId = processingJobId }, resourceStatus);
        }
        // GET: api/Job/GetStatus/123
        //We have assigned the Route Name as GetJobStatus
        [HttpGet("{JobId}", Name = "GetJobStatus")]
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