using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ResultFilter.Filter;

namespace ResultFilter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DemoController : ControllerBase
    {
         // Phương thức action để minh họa việc sử dụng filter Bọc Response Toàn cục
        // Route: GET api/demo/wrapped-response
        // Áp dụng ApiResponseWrapperResultFilter để bọc response trong một định dạng chuẩn
        [HttpGet("wrapped-response")]
        [TypeFilter(typeof(ApiResponseWrapperResultFilter))]
        public IActionResult GetWrappedResponse()
        {
            // Chuẩn bị dữ liệu mẫu để trả về
            var data = new { Id = 1, Name = "John Doe" };
            // Trả về HTTP 200 OK với dữ liệu; filter sẽ bọc response này
            return Ok(data);
        }

        // Phương thức action để minh họa việc thêm/sửa đổi HTTP response header
        // Route: GET api/demo/headers
        // Áp dụng AddCustomHeadersResultFilter để thêm các header như X-API-Version và Cache-Control
        [HttpGet("headers")]
        [TypeFilter(typeof(AddCustomHeadersResultFilter))]
        public IActionResult GetWithCustomHeaders()
        {
            // Chuẩn bị thông điệp mẫu để trả về
            var data = new { Message = "Headers should be modified in the response." };
            // Trả về HTTP 200 OK với thông điệp; filter sẽ thêm các header tùy chỉnh vào response này
            return Ok(data);
        }

        // Phương thức action để mô phỏng một response lớn để kiểm tra filter nén
        // Route: GET api/demo/large-response
        // Áp dụng ResponseCompressionResultFilter để nén các response JSON lớn hơn 100 KB
        [HttpGet("large-response")]
        [TypeFilter(typeof(ResponseCompressionResultFilter))]
        public IActionResult GetLargeResponse()
        {
            // Tạo một chuỗi lớn kích thước 150 KB (150 * 1024 ký tự)
            var largeData = new string('A', 150 * 1024);
            // Trả về dữ liệu lớn được bọc trong một đối tượng; filter sẽ nén response này
            return Ok(new { Content = largeData });
        }

        // Phương thức action để minh họa việc ghi log kiểm toán các response
        // Route: GET api/demo/audit-logging
        // Áp dụng AuditLoggingResultFilter để ghi log chi tiết response sau khi thực thi
        [HttpGet("audit-logging")]
        [TypeFilter(typeof(AuditLoggingResultFilter))]
        public IActionResult GetForAuditLogging()
        {
            // Chuẩn bị một danh sách các mục mẫu
            var info = new List<string> { "Entry1", "Entry2", "Entry3" };
            // Trả về HTTP 200 OK với danh sách; filter sẽ ghi log chi tiết của response này
            return Ok(info);
        }

        // Phương thức action minh họa việc kết hợp nhiều filter trên một action duy nhất
        // Route: GET api/demo/combined
        // Áp dụng các filter bọc response, sửa đổi header, và ghi log kiểm toán cùng nhau
        [HttpGet("combined")]
        [TypeFilter(typeof(ApiResponseWrapperResultFilter))]
        [TypeFilter(typeof(AddCustomHeadersResultFilter))]
        [TypeFilter(typeof(AuditLoggingResultFilter))]
        public IActionResult GetCombined()
        {
            // Chuẩn bị dữ liệu trạng thái mẫu
            var data = new { Status = "Multiple filters applied" };
            // Trả về HTTP 200 OK với dữ liệu; tất cả các filter đã chỉ định sẽ chạy theo thứ tự trên response này
            return Ok(data);
        }
    }
}