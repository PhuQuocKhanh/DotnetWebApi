using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NLogDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        // Inject ILogger thông qua Dependency Injection
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("all-logs")]
        public IActionResult LogAllLevels()
        {
            // Ghi log mức Trace
            _logger.LogTrace("LogTrace: Vào endpoint LogAllLevels với mức Trace.");
            int calculation = 5 * 10;
            _logger.LogTrace("LogTrace: Giá trị calculation là {calculation}", calculation);

            // Ghi log mức Debug
            _logger.LogDebug("LogDebug: Bắt đầu log debug.");
            var debugInfo = new { Action = "LogAllLevels", Status = "Debugging" };
            _logger.LogDebug("LogDebug: Thông tin debug: {@debugInfo}", debugInfo);

            // Ghi log mức Information
            _logger.LogInformation("LogInformation: Endpoint LogAllLevels được gọi thành công.");

            // Ghi log mức Warning
            bool resourceLimitApproaching = true;
            if (resourceLimitApproaching)
            {
                _logger.LogWarning("LogWarning: Tài nguyên sắp đạt giới hạn, cần xử lý sớm.");
            }

            // Ghi log mức Error (kèm Exception)
            try
            {
                int x = 0;
                int result = 10 / x; // lỗi chia cho 0
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogError: Có lỗi xảy ra khi xử lý request.");
            }

            // Ghi log mức Critical
            bool criticalFailure = true;
            if (criticalFailure)
            {
                _logger.LogCritical("LogCritical: Lỗi nghiêm trọng, cần xử lý ngay.");
            }

            return Ok("Đã ghi log ở tất cả các mức.");
        }
    }
}