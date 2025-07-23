using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyFirstWebAPIProject.Middlewares
{
    public class CustomAuthentication403Middleware
    {
        // Field to hold the next middleware component in the pipeline
        private readonly RequestDelegate _next;
        // Constructor to initialize the middleware with the next delegate
        public CustomAuthentication403Middleware(RequestDelegate next)
        {
            _next = next; // Assign the next delegate to the field
        }
        // The main method called by the ASP.NET Core pipeline
        public async Task Invoke(HttpContext context)
        {
            // Custom logic to determine if the user is authorized
            bool isAuthorized = CheckUserAuthorization(context);
            // If the user is not authorized
            if (!isAuthorized)
            {
                // Set the HTTP status code to 403 Forbidden
                context.Response.StatusCode = StatusCodes.Status403Forbidden;

                // Set the response content type to JSON
                context.Response.ContentType = "application/json";
                // Create a custom response object
                var customResponse = new
                {
                    Code = 403, // Custom code for forbidden status
                    Message = "Access is denied due to insufficient permissions." // Custom message
                };
                // Serialize the custom response object to JSON
                var responseJson = JsonSerializer.Serialize(customResponse);
                // Write the JSON response to the HTTP response body
                await context.Response.WriteAsync(responseJson);
                return; // Short-circuit the middleware pipeline
            }
            // If the user is authorized, call the next middleware in the pipeline
            await _next(context);
        }
        
        // Custom method to check user authorization
        private bool CheckUserAuthorization(HttpContext context)
        {
            // Implement your authorization logic here
            // For this example, returning false to simulate a forbidden scenario
            return false;
        }
    }
}