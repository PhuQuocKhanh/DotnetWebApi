using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ServiceFilter_TypeFilter.Filters
{
    public class CustomValidationFilter : IActionFilter
    {
        private readonly string _requiredHeader;
        private readonly ILogger<CustomValidationFilter> _logger;

        public CustomValidationFilter(string requiredHeader, ILogger<CustomValidationFilter> logger)
        {
            _requiredHeader = requiredHeader;
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Headers.ContainsKey(_requiredHeader))
            {
                _logger.LogWarning("Missing required header: {Header}", _requiredHeader);
                context.Result = new BadRequestObjectResult($"Missing required header: {_requiredHeader}");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Không làm gì
        }
    }
}