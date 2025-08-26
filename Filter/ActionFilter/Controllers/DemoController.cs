using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionFilter.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ActionFilter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DemoController : ControllerBase
    {
        // Endpoint này minh họa việc ghi log Request và Response
        // RequestResponseLoggingFilter được áp dụng ở đây bằng attribute TypeFilter để hỗ trợ dependency injection
        [HttpGet("log-request-response")]                      // Phương thức HTTP GET được ánh xạ tới route api/demo/log-request-response
        [TypeFilter(typeof(RequestResponseLoggingFilter))]    // Gắn RequestResponseLoggingFilter tùy chỉnh vào action này
        public IActionResult LogRequestResponseDemo([FromQuery] string sampleParam)  // Chấp nhận một tham số query string 'sampleParam'
        {
            // Trả về HTTP 200 OK với một response JSON đơn giản xác nhận việc ghi log đã hoạt động và lặp lại tham số query
            return Ok(new { Message = "Request and response logged successfully.", Param = sampleParam });
        }
        // Endpoint này minh họa việc sử dụng filter xác thực đầu vào
        // ComplexInputValidationFilter được áp dụng qua attribute TypeFilter cho DI
        [HttpGet("validate-input")]                             // Phương thức HTTP GET được ánh xạ tới api/demo/validate-input
        [TypeFilter(typeof(ComplexInputValidationFilter))]     // Gắn ComplexInputValidationFilter để xác thực đầu vào trước khi action chạy
        public IActionResult ValidateInputDemo([FromQuery] DateTime StartDate, [FromQuery] DateTime EndDate) // Chấp nhận tham số query StartDate và EndDate
        {
            // Trả về 200 OK với JSON xác nhận validation thành công và lặp lại các ngày đầu vào
            return Ok(new { Message = "Input validated successfully.", StartDate, EndDate });
        }
        // Endpoint này minh họa việc bọc Response API theo chuẩn
        // Sử dụng trực tiếp attribute APIResponseWrapperFilter để tự động định dạng response
        [HttpGet("standardized-response")]                       // Phương thức HTTP GET được ánh xạ tới api/demo/standardized-response
        [APIResponseWrapperFilter]                               // Áp dụng attribute filter tùy chỉnh để bọc response theo một định dạng chuẩn
        public IActionResult StandardizedResponseDemo()
        {
            // Đối tượng dữ liệu mẫu được trả về từ action method
            var data = new { Value = 123, Description = "Some data" };
            // Trả về 200 OK với dữ liệu mẫu; filter response sẽ tự động bọc response này
            return Ok(data);
        }
        // Endpoint này minh họa việc sử dụng filter ghi log thời gian thực thi
        // ExecutionTimeLoggingFilter được áp dụng qua attribute TypeFilter để đo thời gian thực thi
        [HttpGet("execution-time")]                              // Phương thức HTTP GET được ánh xạ tới api/demo/execution-time
        [TypeFilter(typeof(ExecutionTimeLoggingFilter))]        // Gắn filter ghi log thời gian thực thi tùy chỉnh
        public IActionResult ExecutionTimeDemo()
        {
            // Mô phỏng độ trễ xử lý 500 mili giây để giả lập một số công việc
            Thread.Sleep(500);
            // Trả về 200 OK với một thông báo xác nhận đơn giản
            return Ok(new { Message = "Execution time logged." });
        }
    }
}