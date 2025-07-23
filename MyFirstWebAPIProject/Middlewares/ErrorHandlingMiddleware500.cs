using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyFirstWebAPIProject.Middlewares
{
    public class ErrorHandlingMiddleware500
    {
        // Delegate representing the next middleware in the pipeline
        private readonly RequestDelegate _next; 
        // Constructor to initialize the middleware with the next delegate in the pipeline
        public ErrorHandlingMiddleware500(RequestDelegate next)
        {
            _next = next;
        }
        // Method to invoke the middleware logic
        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Try to process the request by passing it to the next middleware
                await _next(context);
            }
            catch (Exception ex)
            {
                // If an exception is thrown, handle it here
                // Set the response status code to 500 Internal Server Error
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                // Set the response content type to application/json
                context.Response.ContentType = "application/json";
                // Create a custom error response object
                var customErrorResponse = new
                {
                    Code = 500,
                    Message = "Internal Server Error Occurred",
                    ExceptionDetails = ex.Message // Include exception message for debugging purposes
                };
                // Log the Exception Details (logging not implemented here, but this comment indicates where it should be done)
                // Serialize the custom error response object to JSON
                var responseJson = JsonSerializer.Serialize(customErrorResponse);
                // Write the JSON response to the HTTP response body
                await context.Response.WriteAsync(responseJson);
            }
        }
    }
}