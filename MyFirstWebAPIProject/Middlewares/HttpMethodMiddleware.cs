using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyFirstWebAPIProject.Middlewares
{
public class HttpMethodMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] _allowedMethods;

        public HttpMethodMiddleware(RequestDelegate next, string[] allowedMethods)
        {
            _next = next;
            _allowedMethods = allowedMethods;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!_allowedMethods.Contains(context.Request.Method))
            {
                context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
                context.Response.ContentType = "application/json";

                var customResponse = new
                {
                    Code = 405,
                    Message = "HTTP Method not allowed"
                };

                var responseJson = JsonSerializer.Serialize(customResponse);
                await context.Response.WriteAsync(responseJson);
                return;
            }

            await _next(context);
        }
    }
}