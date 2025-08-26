using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ActionFilter.Filters
{
    // Action filter bất đồng bộ tùy chỉnh kế thừa từ ActionFilterAttribute
    public class ExecutionTimeLoggingFilter : ActionFilterAttribute
    {
        // Instance Stopwatch để đo thời gian trôi qua; có thể là null để cho phép khởi tạo lại
        private Stopwatch? _stopwatch;
        // Instance ILogger để ghi log thông tin thời gian thực thi, được inject qua constructor
        private readonly ILogger<ExecutionTimeLoggingFilter> _logger;
        // Constructor nhận ILogger<ExecutionTimeLoggingFilter> qua Dependency Injection
        public ExecutionTimeLoggingFilter(ILogger<ExecutionTimeLoggingFilter> logger)
        {
            _logger = logger; // Gán logger vào trường private
        }
        // Phương thức override bất đồng bộ chạy trước và sau khi action method thực thi
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Bắt đầu một Stopwatch mới để tính thời gian thực thi của action
            _stopwatch = Stopwatch.StartNew();
            // Gọi delegate/middleware tiếp theo trong pipeline, chính là chạy action method
            var executedContext = await next();
            // Dừng Stopwatch sau khi action thực thi xong
            _stopwatch.Stop();
            // Lấy tên hiển thị của action method đang được thực thi (để ghi log)
            var actionName = context.ActionDescriptor.DisplayName;
            // Ghi log thời gian đã trôi qua (tính bằng mili giây) cho việc thực thi action
            _logger.LogInformation($"Execution Time for {actionName} : {_stopwatch.ElapsedMilliseconds} ms");
        }
    }
}