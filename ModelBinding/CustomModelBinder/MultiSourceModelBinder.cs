using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ModelBinding.Models;
using Newtonsoft.Json;

namespace ModelBinding.CustomModelBinder
{
    public class MultiSourceModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            try
            {
                // Accessing the HttpContext (HTTP request/response) for the current model binding operation.
                var httpContext = bindingContext.HttpContext;
                // Error handling: Check if the request contains necessary data sources like headers, query, and body.
                if (httpContext.Request.Headers == null || httpContext.Request.Query == null || httpContext.Request.Body == null)
                {
                    // If any of the sources are missing, add an error to the ModelState and mark the binding as failed.
                    bindingContext.ModelState.AddModelError("MultiSourceModelBinder", "Missing necessary data in the request.");
                    bindingContext.Result = ModelBindingResult.Failed();
                    return;  // Exit the method if any data source is missing.
                }
                // Fetching a custom header value ("X-Custom-Header") from the HTTP request headers and converting it to a string.
                var headerValue = httpContext.Request.Headers["X-Custom-Header"].ToString();
                // Fetching a value from the HTTP request query string ("queryValue") and converting it to a string.
                var queryValue = httpContext.Request.Query["queryValue"].ToString();
                // Error handling: Validate header and query values to ensure they are not empty or null.
                if (string.IsNullOrEmpty(headerValue))
                {
                    // Add an error to ModelState if the header value is missing or empty.
                    bindingContext.ModelState.AddModelError("Header", "X-Custom-Header is missing or empty.");
                    bindingContext.Result = ModelBindingResult.Failed();  // Mark the binding as failed.
                    return;  // Exit the method if the header value is invalid.
                }
                if (string.IsNullOrEmpty(queryValue))
                {
                    // Add an error to ModelState if the query value is missing or empty.
                    bindingContext.ModelState.AddModelError("Query", "Query parameter 'queryValue' is missing or empty.");
                    bindingContext.Result = ModelBindingResult.Failed();  // Mark the binding as failed.
                    return;  // Exit the method if the query value is invalid.
                }
                // Reading the entire body content of the HTTP request asynchronously and converting it to a string.
                string bodyContent;
                try
                {
                    // Use a StreamReader to read the request body stream asynchronously.
                    using (var reader = new StreamReader(httpContext.Request.Body))
                    {
                        bodyContent = await reader.ReadToEndAsync();  // Read the body content as a string.
                    }
                }
                catch (Exception ex)
                {
                    // If there is an error reading the body (e.g., stream issues), add an error to ModelState.
                    bindingContext.ModelState.AddModelError("Body", $"Error reading request body: {ex.Message}");
                    bindingContext.Result = ModelBindingResult.Failed();  // Mark the binding as failed.
                    return;  // Exit the method if there is an error reading the body.
                }
                // Deserializing the JSON string from the request body into a ComplexBodyModel object.
                ComplexBodyModel bodyModel;
                try
                {
                    // Deserialize the JSON body content into the ComplexBodyModel class.
                    bodyModel = JsonConvert.DeserializeObject<ComplexBodyModel>(bodyContent);
                    // If the deserialization returns null, indicate a failure in deserializing the body.
                    if (bodyModel == null)
                    {
                        bindingContext.ModelState.AddModelError("Body", "Failed to deserialize the body content into ComplexBodyModel.");
                        bindingContext.Result = ModelBindingResult.Failed();  // Mark the binding as failed.
                        return;  // Exit the method if deserialization fails.
                    }
                }
                catch (JsonException jsonEx)
                {
                    // If the JSON deserialization throws an exception (e.g., invalid JSON format), handle it here.
                    bindingContext.ModelState.AddModelError("Body", $"Invalid JSON format in request body: {jsonEx.Message}");
                    bindingContext.Result = ModelBindingResult.Failed();  // Mark the binding as failed.
                    return;  // Exit the method if deserialization fails.
                }
                // Creating a new instance of MergedModel and populating its properties with data from header, query, and body.
                var mergedModel = new MergedModel
                {
                    Header = headerValue,  // Setting the Header property with the value obtained from the request header.
                    Query = queryValue,    // Setting the Query property with the value obtained from the request query string.
                    BodyData = bodyModel?.Data  // Setting the BodyData property with the "Data" from the deserialized body content, using null-conditional operator to handle potential null values.
                };
                // Marking the model binding as successful and passing the merged model back to the framework.
                bindingContext.Result = ModelBindingResult.Success(mergedModel);
            }
            catch (Exception ex)
            {
                // General error handling: Catch any unexpected exceptions and report failure.
                bindingContext.ModelState.AddModelError("MultiSourceModelBinder", $"An unexpected error occurred: {ex.Message}");
                bindingContext.Result = ModelBindingResult.Failed();  // Mark the binding as failed.
            }
        }
    }
}