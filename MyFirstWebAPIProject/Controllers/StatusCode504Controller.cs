using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyFirstWebAPIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StatusCode504Controller : ControllerBase
    {
        private readonly HttpClient _httpClient;
        public StatusCode504Controller(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
         // Action GET để gọi một API bên ngoài
        [HttpGet]
        public async Task<IActionResult> GetResource()
        {
            var requestUrl = "https://httpstat.us/504"; // URL của API ngoài trả về lỗi 504
            try
            {
                var response = await _httpClient.GetAsync(requestUrl);

                // Kiểm tra nếu mã phản hồi là 504 Gateway Timeout
                if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                {
                    var customResponse = new
                    {
                        Code = 504,
                        Message = "Yêu cầu đã vượt quá thời gian chờ từ dịch vụ bên ngoài."
                    };

                    var responseJson = JsonSerializer.Serialize(customResponse);
                    return StatusCode(504, responseJson); // Trả về mã 504 với nội dung JSON tùy chỉnh
                }

                // Nếu gọi API thành công
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return Ok(content); // Trả về nội dung cùng với mã 200 OK
                }
                else
                {
                    // Trả về mã lỗi gốc từ API ngoài nếu không phải thành công hay 504
                    return StatusCode((int)response.StatusCode, "Không thể truy xuất tài nguyên từ dịch vụ ngoài.");
                }
            }
            catch (Exception ex)
            {
                // Trong trường hợp xảy ra exception nội bộ (ví dụ: lỗi mạng)
                var customResponse = new
                {
                    Code = 500,
                    Message = $"Lỗi máy chủ nội bộ: {ex.Message}"
                };

                var responseJson = JsonSerializer.Serialize(customResponse);
                return StatusCode(500, responseJson); // Trả về mã 500 với nội dung chi tiết lỗi
            }
        }
    }
}