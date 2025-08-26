using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ActionFilter.Filters
{
    // Action filter tùy chỉnh kế thừa từ ActionFilterAttribute để sử dụng dưới dạng attribute dễ dàng
    public class APIResponseWrapperFilter : ActionFilterAttribute
    {
        // Phương thức này chạy sau khi action method đã thực thi
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // Kiểm tra xem action có trả về một ObjectResult (response JSON/dữ liệu thông thường) không
            if (context.Result is ObjectResult objectResult)
            {
                // Tạo một đối tượng vô danh mới để bọc giá trị response ban đầu
                // Chuẩn hóa định dạng response để bao gồm các trường Status, Message, và Data
                var wrappedResponse = new
                {
                    Status = objectResult.StatusCode ?? 200, // Sử dụng status code hiện có hoặc mặc định là 200 OK
                    Message = "Success",                      // Thông báo thành công cố định
                    Data = objectResult.Value                 // Payload dữ liệu của response gốc
                };
                // Thay thế result ban đầu bằng một JsonResult mới bọc response đã được chuẩn hóa
                context.Result = new JsonResult(wrappedResponse)
                {
                    StatusCode = objectResult.StatusCode // Giữ lại status code gốc trong response mới
                };
            }
            // Nếu action trả về một EmptyResult (không có nội dung để gửi)
            else if (context.Result is EmptyResult)
            {
                // Tạo một response được bọc cho biết không có nội dung
                var wrappedResponse = new
                {
                    Status = 204,          // HTTP 204 No Content status
                    Message = "No content",// Thông báo mô tả cho trường hợp không có nội dung
                    Data = null as object  // Payload dữ liệu là null
                };
                // Thay thế EmptyResult ban đầu bằng một JsonResult bọc response không có nội dung đã được chuẩn hóa
                context.Result = new JsonResult(wrappedResponse)
                {
                    StatusCode = 204       // Đặt status HTTP thành 204 No Content
                };
            }
            // Gọi phương thức của lớp cơ sở để đảm bảo mọi xử lý bổ sung được thực hiện
            base.OnActionExecuted(context);
        }
    }
}