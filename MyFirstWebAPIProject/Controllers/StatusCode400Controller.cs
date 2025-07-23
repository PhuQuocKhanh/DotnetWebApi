using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFirstWebAPIProject.Models;

namespace MyFirstWebAPIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StatusCode400Controller : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            // Tự kiểm tra tính hợp lệ của model
            if (!ModelState.IsValid)
            {
                // Tạo response lỗi tùy chỉnh
                var errorResponse = new { error = "Invalid Request Data" };
                return new BadRequestObjectResult(errorResponse); // Trả về 400 với thông tin tùy chỉnh
            }

            // Trả về 200 nếu dữ liệu hợp lệ
            return Ok();
        }

        // Define a POST endpoint for creating an employee
        [HttpPost]
        public IActionResult ProblemDetails([FromBody] Employee employee)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                // Create a dictionary to hold the details of each validation error
                var errors = ModelState
                    .Where(e => e.Value?.Errors.Count > 0) // Filter model state entries that have errors
                    .ToDictionary(
                        kvp => kvp.Key, // Use the property name as the key
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() // Use the error messages as the value
                    );
                // Create a ProblemDetails object to hold the error response details
                var problemDetails = new ProblemDetails()
                {
                    Title = "Validation Errors Occurred.", // Short description of the problem
                    Detail = "See the errors property for details", // Detailed description of the problem
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1", // URI reference for the problem type
                    Status = StatusCodes.Status400BadRequest, // HTTP status code for the response
                    Instance = HttpContext.Request.Path, // The path to the request that generated the error
                    Extensions = new Dictionary<string, object?> // Additional custom data
                    {
                        { "Retry", "Please Retry After 30 Minutes" }, // Custom retry message
                        { "errors", errors } // Include the detailed validation errors
                    }
                };
                // Return a 400 Bad Request response with the problem details
                return BadRequest(problemDetails);
            }
            // Process the request (e.g., save the employee data to the database)
            // ...
            // Return a 200 OK response indicating success
            return Ok();
        }
        // Define a POST endpoint for creating an employee
        [HttpPost]
        public IActionResult ValidationProblemDetails ([FromBody] Employee employee)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                // Returns a 400 Status Code with model state errors
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Title = "Validation Errors Occurred.", // Short description of the problem
                    Detail = "See the errors property for details", // Detailed description of the problem
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1", // URI reference for the problem type
                    Status = StatusCodes.Status400BadRequest, // HTTP status code for the response
                    Instance = HttpContext.Request.Path, // The path to the request that generated the error
                };
                
                // Return a 400 Bad Request response with the problem details
                return BadRequest(problemDetails);
            }
            // Process the request (e.g., save the employee data to the database)
            // ...
            // Return a 200 OK response indicating success
            return Ok();
        }
    }
}