using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CustomLoggingProvider.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("LogAllLevels")]
        public IActionResult LogAllLevels()
        {
            //Storing the Optional EventId
            var eventId = new EventId(1000);
            _logger.LogTrace("LogTrace: Entering the LogAllLevels endpoint with Trace-level logging.");
            // Simulate a variable and log it at Trace level
            int calculation = 5 * 10;
            _logger.LogTrace("LogTrace: Calculation value is {calculation}", calculation);
            _logger.LogDebug("LogDebug: Initializing debug-level logs for debugging purposes.");
            // Log some debug information
            var debugInfo = new { Action = "LogAllLevels", Status = "Debugging" };
            _logger.LogDebug("LogDebug: Debug information: {@debugInfo}", debugInfo);
            _logger.LogInformation(eventId, "LogInformation: The LogAllLevels endpoint was reached successfully.");
            // Simulate a condition that might cause an issue
            bool IsTakingMoreTime = true;
            if (IsTakingMoreTime)
            {
                _logger.LogWarning(eventId, "LogWarning: External API taking More Time to Respond. Action may be required soon.");
            }
            try
            {
                // Simulate an error scenario
                int x = 0;
                int result = 10 / x;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogError: An error occurred while processing the request.");
            }
            // Log a critical error scenario
            bool criticalFailure = true;
            if (criticalFailure)
            {
                _logger.LogCritical("LogCritical: A critical system failure has been detected. Immediate attention is required.");
            }
            return Ok("All logging levels demonstrated in this endpoint.");
        }
    }
}