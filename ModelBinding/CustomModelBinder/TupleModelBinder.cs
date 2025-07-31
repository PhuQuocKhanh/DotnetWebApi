using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace ModelBinding.CustomModelBinder
{
    public class TupleModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // Ensure that bindingContext is not null to avoid NullReferenceException
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }
            try
            {
                // Read the request body asynchronously and convert it to a string
                var body = await new StreamReader(bindingContext.HttpContext.Request.Body).ReadToEndAsync();
                // Check if the request body is empty
                if (string.IsNullOrEmpty(body))
                {
                    // Set the result to failure and add an error message if the body is empty and exit the method
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Request body is empty.");
                    bindingContext.Result = ModelBindingResult.Failed();
                    return;
                }
                // Deserialize the JSON body into a Tuple<int, string>
                var tupleData = JsonConvert.DeserializeObject<Tuple<int, string>>(body);
                // Check if deserialization succeeded, if null, return failed model binding and exit the method
                if (tupleData == null)
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalid JSON format or type mismatch.");
                    bindingContext.Result = ModelBindingResult.Failed();
                    return;
                }
                // Set the binding result as success if deserialization is successful
                bindingContext.Result = ModelBindingResult.Success(tupleData);
            }
            catch (JsonException jsonEx)
            {
                // Handle JSON-specific errors (e.g., invalid JSON)
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, $"JSON deserialization error: {jsonEx.Message}");
                bindingContext.Result = ModelBindingResult.Failed();
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, $"An error occurred: {ex.Message}");
                bindingContext.Result = ModelBindingResult.Failed();
            }
        }
    }
}