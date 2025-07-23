using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyFirstWebAPIProject.Filter
{
    public class HttpMethodFilter : IActionFilter
    {
         // Biến private lưu danh sách các HTTP method được phép.
        private readonly string[] _allowedMethods;

        // Constructor nhận vào danh sách các method được phép.
        public HttpMethodFilter(string[] allowedMethods)
        {
            _allowedMethods = allowedMethods;
        }

        // Hàm được gọi trước khi action thực thi.
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Kiểm tra nếu HTTP method hiện tại không nằm trong danh sách cho phép.
            if (!_allowedMethods.Contains(context.HttpContext.Request.Method))
            {
                // Tạo response lỗi tùy chỉnh.
                var customResponse = new
                {
                    Code = 405, // Mã trạng thái HTTP: Method Not Allowed
                    Message = "HTTP Method not allowed" // Thông báo lỗi tùy chỉnh
                };

                // Gán kết quả trả về là một ObjectResult với mã lỗi 405.
                context.Result = new ObjectResult(customResponse)
                {
                    StatusCode = 405
                };
            }
        }

        // Hàm được gọi sau khi action thực thi xong.
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Hàm này bắt buộc phải có do giao diện IActionFilter yêu cầu,
            // nhưng trong ví dụ này không cần xử lý gì sau khi action thực thi.
        }
    }
} 