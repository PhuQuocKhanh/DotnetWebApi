using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LoggingDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestLoggingController : ControllerBase
    {
        private readonly ILogger<TestLoggingController> _logger;
        public TestLoggingController(ILogger<TestLoggingController> logger)
        {
            _logger = logger;
        }

        [HttpGet("all-logs")]
        public IActionResult LogAllLevels()
        {
            _logger.LogTrace("LogTrace: Vào endpoint LogAllLevels với log mức Trace.");
            int calculation = 5 * 10;
            _logger.LogTrace("LogTrace: Giá trị phép tính là {calculation}", calculation);

            _logger.LogDebug("LogDebug: Ghi log debug để kiểm tra hoạt động.");
            var debugInfo = new { Action = "LogAllLevels", Status = "Debugging" };
            _logger.LogDebug("LogDebug: Thông tin debug: {@debugInfo}", debugInfo);

            _logger.LogInformation("LogInformation: Đã truy cập endpoint LogAllLevels thành công.");

            bool IsTakingMoreTime = true;
            if (IsTakingMoreTime)
            {
                _logger.LogWarning("LogWarning: API ngoài phản hồi chậm. Cần xem xét.");
            }

            try
            {
                int x = 0;
                int result = 10 / x; // Lỗi chia cho 0
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogError: Lỗi xảy ra trong quá trình xử lý request.");
            }

            bool criticalFailure = true;
            if (criticalFailure)
            {
                _logger.LogCritical("LogCritical: Phát hiện lỗi hệ thống nghiêm trọng. Cần xử lý ngay.");
            }

            return Ok("Đã minh họa tất cả các cấp độ log.");
        }
    }
}