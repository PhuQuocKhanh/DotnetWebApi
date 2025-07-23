using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFirstWebAPIProject.Services;
using Polly.Retry;

namespace MyFirstWebAPIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StatusCode503Controller : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IRetryPolly _retryPolicy;

        public StatusCode503Controller(IRetryPolly retryPolicy, IConfiguration configuration)
        {
            _retryPolicy = retryPolicy;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult ReturnStatusCode503()
        {
            // Đọc giá trị cấu hình từ appsettings.json
            bool isUnderMaintenance = Convert.ToBoolean(_configuration["IsApplicationUnderMaintenance"]);

            // Nếu đang bảo trì, trả về HTTP 503
            if (isUnderMaintenance)
            {
                var customResponse = new
                {
                    Code = 503,
                    Message = "Service is under maintenance. Please try again later."
                };

                return StatusCode(503, customResponse);
            }

            // Nếu không bảo trì, trả về HTTP 200
            return Ok("Service is Available");
        }

        [HttpGet]
        public async Task<IActionResult> GetExternalData()
        {
            try
            {
                var data = await _retryPolicy.GetExternalDataAsync();
                return Ok(data); // 200 OK
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
            {
                // Xử lý lỗi 503 Service Unavailable
                var customResponse = new
                {
                    Code = 503,
                    Message = "Service is temporarily unavailable. Please try again later.",
                };
                return StatusCode(503, customResponse);
            }
            catch (Exception)
            {
                // Xử lý lỗi 500 Internal Server Error
                var customResponse = new
                {
                    Code = 500,
                    Message = "An internal server error occurred. Please try again later.",
                };
                return StatusCode(500, customResponse);
            }
        }
    }
}