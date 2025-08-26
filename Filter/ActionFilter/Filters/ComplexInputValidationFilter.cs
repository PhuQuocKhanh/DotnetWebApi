using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ActionFilter.Filters
{
    // Action filter bất đồng bộ tùy chỉnh triển khai interface IAsyncActionFilter
    public class ComplexInputValidationFilter : IAsyncActionFilter
    {
        // Phương thức này chạy bất đồng bộ trước và sau khi action method thực thi
        // 'context' cung cấp thông tin về request hiện tại và các tham số của action
        // delegate 'next' được sử dụng để gọi action filter tiếp theo hoặc chính action method
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Thử lấy tham số "StartDate" từ các tham số của action method
            // và gán nó vào 'startDateObj' nếu tồn tại
            // Tương tự, thử lấy tham số "EndDate" và gán vào 'endDateObj'
            if (context.ActionArguments.TryGetValue("StartDate", out var startDateObj) &&
                context.ActionArguments.TryGetValue("EndDate", out var endDateObj) &&
                // Kiểm tra xem cả hai đối tượng có phải là kiểu DateTime không, và ép kiểu tương ứng
                startDateObj is DateTime startDate && endDateObj is DateTime endDate)
            {
                // Xác thực rằng StartDate KHÔNG được sau EndDate
                if (startDate > endDate)
                {
                    // Nếu validation thất bại, đặt context.Result thành một BadRequestObjectResult
                    // Điều này tạo ra một response HTTP 400 Bad Request với payload là JSON
                    context.Result = new BadRequestObjectResult(new
                    {
                        Status = 400,
                        Message = "StartDate cannot be later than EndDate."
                    });
                    // Ngắt mạch pipeline tại đây, ngăn không cho action method thực thi
                    return;
                }
            }
            // Nếu validation thành công hoặc các tham số không tồn tại, tiếp tục đến filter tiếp theo hoặc action method
            await next();
        }
    }
}