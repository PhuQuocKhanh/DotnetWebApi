using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFirstWebAPIProject.CustomActionResult;

namespace MyFirstWebAPIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StatusCode501Controller : ControllerBase
    {
        private readonly IConfiguration _configuration;
        // Constructor to initialize the configuration dependency
        public StatusCode501Controller(IConfiguration configuration)
        {
            _configuration = configuration;
        }

       // Scenario 1: Unsupported HTTP Method
        [HttpPost("unsupported-method")]
        public IActionResult UnsupportedMethod()
        {
            bool isPostAllowed = _configuration.GetValue<bool>("SupportedMethods:AllowPost", true);
            if (!isPostAllowed)
            {
                var detail = new
                {
                    Message = "POST method is currently disabled on this endpoint.",
                    SupportContact = "support@example.com",
                    Timestamp = DateTime.UtcNow
                };
                // Return custom 501 status code with detailed information
                return new NotImplementedResult(detail);
            }
            // Simulate a typical POST handling logic and return success response
            return Ok(new { Status = "Success", Description = "POST method is accepted and processed." });
        }
        // Scenario 2: Feature Not Implemented
        [HttpGet("feature-not-implemented")]
        public IActionResult FeatureNotImplemented()
        {
            bool isFeatureReady = _configuration.GetValue<bool>("Features:BetaFeatureEnabled", false);
            if (!isFeatureReady)
            {
                var detail = new
                {
                    Message = "Feature is under development and not available.",
                    ExpectedReleaseDate = "2024-10-11"
                };
                // Return custom 501 status code with detailed information
                return new NotImplementedResult(detail);
            }
            // Simulate a response for a ready feature and return success response
            return Ok("Beta feature is fully operational.");
        }
        // Scenario 3: Routing Issues
        [HttpGet("routing-check")]
        public IActionResult RoutingCheck()
        {
            bool isSpecialRouteActive = _configuration.GetValue<bool>("Routing:UseSpecialRoute", false);
            if (!isSpecialRouteActive)
            {
                var detail = new
                {
                    Message = "The requested endpoint is not routed correctly.",
                    HelpUrl = "http://example.com/api-help/routing-issues"
                };
                // Return custom 501 status code with detailed information
                return new NotImplementedResult(detail);
            }
            // Return success response indicating the routing is correctly configured
            return Ok("Routing is correctly configured for this endpoint.");
        }
        // Scenario 4: Misconfiguration
        [HttpGet("configuration-check")]
        public IActionResult ConfigurationCheck()
        {
            string? criticalConfig = _configuration.GetValue<string>("CriticalSettings:ApiKey", null);
            if (string.IsNullOrEmpty(criticalConfig))
            {
                var detail = new
                {
                    Message = "Critical API key is not configured.",
                    Administrator = "admin@example.com"
                };
                // Return custom 501 status code with detailed information
                return new NotImplementedResult(detail);
            }
            // Return success response indicating all critical configurations are set properly
            return Ok("All critical configurations are set properly.");
        }
    }
}