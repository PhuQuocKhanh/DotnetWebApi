using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ModelBinding.Middlewares
{
    // A custom middleware class that handles '406 Not Acceptable' responses
    public class CustomNotAcceptableMiddleware
    {
        // Defining a field to store the next middleware in the pipeline
        private readonly RequestDelegate _next;
        // Constructor that takes in the next middleware in the pipeline
        public CustomNotAcceptableMiddleware(RequestDelegate next)
        {
            // Assigning the injected middleware to the private field
            _next = next;
        }
        // Middleware method called 'Invoke', which is executed for every HTTP request
        public async Task Invoke(HttpContext context)
        {
            // Calling the next middleware in the pipeline and waiting for its completion
            await _next(context);
            // Check if the response status code is 406 Not Acceptable
            if (context.Response.StatusCode == StatusCodes.Status406NotAcceptable)
            {
                // Retrieve the "Accept" header from the request, which indicates the client's preferred response formats
                var acceptHeader = context.Request.Headers["Accept"].ToString();
                // Set the Content-Type of the response to 'application/json' to indicate that the response is in JSON format
                context.Response.ContentType = "application/json";
                // Create an anonymous object containing the status code and a custom error message based on the unsupported format
                var response = new
                {
                    Code = StatusCodes.Status406NotAcceptable, // HTTP status code 406
                    ErrorMessage = $"The Requested Format {acceptHeader} is Not Supported." // Custom error message showing the unsupported format
                };
                // Serialize the anonymous object to JSON and write it to the response body
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}