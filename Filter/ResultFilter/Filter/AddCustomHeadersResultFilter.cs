using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ResultFilter.Filter
{
    // Result Filter tùy chỉnh thêm hoặc sửa đổi các HTTP header trong response
    public class AddCustomHeadersResultFilter : ResultFilterAttribute
    {
        // Phương thức này chạy ngay trước khi action result thực thi (trước khi response được gửi)
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            // Lấy đối tượng HttpResponse từ HTTP context hiện tại
            var response = context.HttpContext.Response;

            // Thêm header tùy chỉnh "X-API-Version" với giá trị "1.0" nếu nó chưa tồn tại
            if (!response.Headers.ContainsKey("X-API-Version"))
            {
                response.Headers["X-API-Version"] = "1.0";
            }

            // Thêm header Cache-Control một cách có điều kiện chỉ cho:
            // 1. Yêu cầu HTTP GET (an toàn để cache)
            // 2. Các response mà kết quả là ObjectResult (kết quả JSON thông thường)
            // 3. Mã trạng thái response là 200 OK (thành công)
            if (context.HttpContext.Request.Method == "GET" &&
                context.Result is ObjectResult objectResult &&
                (objectResult.StatusCode ?? 200) == 200)
            {
                // Thêm header Cache-Control để chỉ định caching công khai với thời gian tối đa là 3600 giây (1 giờ)
                response.Headers["Cache-Control"] = "public, max-age=3600";
            }
            
            // Gọi phương thức của lớp cơ sở để đảm bảo bất kỳ logic nào của lớp cha được thực thi
            base.OnResultExecuting(context);
        }
    }
}