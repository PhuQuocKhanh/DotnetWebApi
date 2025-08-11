using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LoggingSerilogToDatabase.Controllers
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
        
        [HttpGet("all-logs-custom")]
        public IActionResult LogAllLevelsCustom()
        {
            string UniqueId = Guid.NewGuid().ToString();
            try
            {
                // First approach: include UniqueId directly in the log message
                _logger.LogInformation("{UniqueId} This is an Information log.", UniqueId);
                _logger.LogWarning("This is a Warning log. UniqueId: {UniqueId}", UniqueId);
                _logger.LogCritical("This is a Critical log, indicating a serious failure.");
                // Second approach: use ForContext to add UniqueId to the log context
                Log.ForContext("UniqueId", UniqueId).Information("Processing Request Information");
                Log.ForContext("UniqueId", UniqueId).Warning("Processing Request Warning");
                // Simulate an error
                int x = 10, y = 0;
                int z = x / y;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{UniqueId} An error occurred.", UniqueId);
                Log.ForContext("UniqueId", UniqueId).Error("Processing Request Error");
            }
            return Ok("Check your logs to see the different logging levels in action!");
        }

        [HttpGet("all-logs")]
        public IActionResult LogAllLevels()
        {
            _logger.LogTrace("LogTrace: Entering the LogAllLevels endpoint with Trace-level logging.");

            int calculation = 5 * 10;
            _logger.LogTrace("LogTrace: Calculation value is {calculation}", calculation);

            _logger.LogDebug("LogDebug: Initializing debug-level logs for debugging purposes.");

            var complexEmployee = new { Id = 10, Name = "Pranaya", Gender = "Male", Department = "IT" };
            _logger.LogInformation("LogInformation: Employee details: {@complexEmployee}", complexEmployee);

            bool resourceLimitApproaching = true;
            if (resourceLimitApproaching)
            {
                _logger.LogWarning("LogWarning: Resource usage is nearing the limit. Action may be required soon.");
            }

            try
            {
                int x = 0;
                int result = 10 / x;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogError: An error occurred while processing the request.");
            }

            bool criticalFailure = true;
            if (criticalFailure)
            {
                _logger.LogCritical("LogCritical: A critical system failure has been detected. Immediate attention is required.");
            }

            return Ok("All logging levels demonstrated in this endpoint.");
        }
    }
}