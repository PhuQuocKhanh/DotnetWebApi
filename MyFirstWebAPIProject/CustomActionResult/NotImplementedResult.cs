using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyFirstWebAPIProject.CustomActionResult
{
    public class NotImplementedResult : IActionResult
    {
        // Field to hold the detailed information to be included in the response
        private readonly object _detail;
        // Constructor to initialize the detail field
        public NotImplementedResult(object detail)
        {
            _detail = detail; // Assign the provided detail to the private field
        }
        // Method to execute the result and write the response
        public async Task ExecuteResultAsync(ActionContext context)
        {
            // Serialize the detail object to a JSON string
            var json = JsonSerializer.Serialize(_detail);
            // Set the response content type to "application/json"
            context.HttpContext.Response.ContentType = "application/json";
            // Set the response status code to 501 Not Implemented
            context.HttpContext.Response.StatusCode = StatusCodes.Status501NotImplemented;
            // Write the JSON string to the response body
            await context.HttpContext.Response.WriteAsync(json);
        }
    }
}