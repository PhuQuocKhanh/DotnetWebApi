using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ResultFilter.Filter
{
    // Result Filter tùy chỉnh để đóng gói các phản hồi API theo một định dạng chuẩn
    public class ApiResponseWrapperResultFilter : IResultFilter
    {
        // Phương thức này chạy ngay trước khi action result thực thi (trước khi response được ghi)
        public void OnResultExecuting(ResultExecutingContext context)
        {
            // Kiểm tra nếu kết quả trả về bởi action là một ObjectResult (thường là dữ liệu JSON)
            if (context.Result is ObjectResult objectResult)
            {
                // Tạo một đối tượng vô danh mới để đóng gói phản hồi gốc
                // Status: Mã trạng thái HTTP (mặc định 200 nếu chưa được đặt)
                // Message: "Success" cho trạng thái 200, ngược lại là "Error"
                // Data: Giá trị gốc được trả về bởi phương thức action
                var wrappedResponse = new
                {
                    Status = objectResult.StatusCode ?? 200,
                    Message = objectResult.StatusCode == 200 ? "Success" : "Error",
                    Data = objectResult.Value
                };
                
                // Thay thế action result gốc bằng một JsonResult mới chứa cấu trúc đã đóng gói
                // Duy trì mã trạng thái gốc trên JsonResult mới
                context.Result = new JsonResult(wrappedResponse)
                {
                    StatusCode = objectResult.StatusCode
                };
            }
            // Nếu kết quả là EmptyResult (không có nội dung trả về)
            else if (context.Result is EmptyResult)
            {
                // Đóng gói nó với một phản hồi chuẩn cho biết không có nội dung
                var wrappedResponse = new
                {
                    Status = 204,               // Mã trạng thái HTTP 204 No Content
                    Message = "No Content",     // Thông báo mô tả
                    Data = (object?)null        // Dữ liệu là null
                };
                
                // Thay thế EmptyResult bằng một JsonResult để duy trì định dạng response nhất quán
                context.Result = new JsonResult(wrappedResponse)
                {
                    StatusCode = 204            // Đặt trạng thái HTTP thành 204 No Content
                };
            }
        }

        // Phương thức này chạy sau khi kết quả đã được thực thi (sau khi response đã được gửi)
        public void OnResultExecuted(ResultExecutedContext context)
        {
            // Tùy chọn cho việc xử lý sau kết quả và hiện tại để trống
        }
    }
}