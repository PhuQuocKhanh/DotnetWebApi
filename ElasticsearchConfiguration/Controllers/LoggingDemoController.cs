using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ElasticsearchConfiguration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoggingDemoController : ControllerBase
    {
        private readonly ILogger<LoggingDemoController> _logger;
        public LoggingDemoController(ILogger<LoggingDemoController> logger)
        {
            _logger = logger;
        }
        [HttpGet("loglevels")]
        public IActionResult LogAllLevels()
        {
            // Trace-level logging: very detailed diagnostic information.
            _logger.LogTrace("LogTrace: Entering the LogAllLevels endpoint with Trace-level logging.");
            // Debug-level logging: useful for debugging during development.
            int calculation = 5 * 10;
            _logger.LogDebug("LogDebug: Calculation value is {calculation}", calculation);
            // Information-level logging with structured data.
            var employeeInfo = new { Id = 1, Name = "Pranaya", Department = "IT" };
            _logger.LogInformation("LogInformation: Employee info: {@employeeInfo}", employeeInfo);
            // Warning-level logging: indicates a potential issue.
            bool isTakingMoreTime = true;
            if (isTakingMoreTime)
            {
                _logger.LogWarning("LogWarning: External API is taking more time to respond. Action may be required soon.");
            }
            try
            {
                // Simulate an error scenario (e.g., division by zero)
                int x = 0;
                int result = 10 / x;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogError: An error occurred while processing the request.");
            }
            // Critical-level logging: indicates a failure in the application that requires immediate attention.
            bool criticalFailure = true;
            if (criticalFailure)
            {
                _logger.LogCritical("LogCritical: A critical system failure has been detected. Immediate attention is required.");
            }
            return Ok("All logging levels demonstrated in this endpoint.");
        }
    }
}