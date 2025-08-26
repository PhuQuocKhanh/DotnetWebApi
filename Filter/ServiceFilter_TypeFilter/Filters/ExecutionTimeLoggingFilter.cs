using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ServiceFilter_TypeFilter.Filters
{
    public class ExecutionTimeLoggingFilter : IActionFilter
    {
        private readonly ILogger<ExecutionTimeLoggingFilter> _logger;
        private Stopwatch? _stopwatch;

        // Tiêm logger qua constructor
        public ExecutionTimeLoggingFilter(ILogger<ExecutionTimeLoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch = Stopwatch.StartNew();
            _logger.LogInformation("Action execution started.");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _stopwatch?.Stop();
            _logger.LogInformation($"Action executed in {_stopwatch?.ElapsedMilliseconds} ms.");
        }
    }
}