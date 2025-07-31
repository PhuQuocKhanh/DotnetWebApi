using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ModelBinding.Models;

namespace ModelBinding.CustomModelBinder
{
    public class LargeDataModelBinder : IModelBinder
    {
        // Asynchronous method that binds the incoming request body to a LargeDataModel object
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // Ensure that bindingContext is not null to avoid NullReferenceException
            if (bindingContext == null)
            {
                // Throws an exception if the binding context is null, ensuring no null reference issues
                throw new ArgumentNullException(nameof(bindingContext));
            }
            try
            {
                // Initialize an empty list to hold the lines of large data from the request body
                var largeDataList = new List<string>();
                // Create a StreamReader to read the request body content line by line
                using (var reader = new StreamReader(bindingContext.HttpContext.Request.Body))
                {
                    // Initialize a string variable to hold each line of the data
                    string? line;
                    // Read the request body line by line asynchronously
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        // Add each non-null line to the list of large data
                        largeDataList.Add(line);
                    }
                }
                // After reading the entire body, check if the list is still empty
                if (largeDataList.Count == 0)
                {
                    // If the list is empty, add an error to ModelState indicating no valid data was found
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Request body is empty or contains no valid data.");
                    // Mark the binding as failed
                    bindingContext.Result = ModelBindingResult.Failed();
                    // Exit the method since binding has failed
                    return;
                }
                // If data was successfully read and the list is not empty, set the binding result as success
                bindingContext.Result = ModelBindingResult.Success(new LargeDataModel { LargeDataList = largeDataList });
            }
            catch (Exception ex)
            {
                // Handle any general exceptions that occur during model binding
                // Add the exception message to ModelState to give details of what went wrong
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, $"An unexpected error occurred: {ex.Message}");
                // Mark the binding result as failed due to the exception
                bindingContext.Result = ModelBindingResult.Failed();
            }
        }
    }
}