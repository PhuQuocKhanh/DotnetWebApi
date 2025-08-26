using Microsoft.AspNetCore.Mvc;
using ResourceFilters.Filters;

namespace ResourceFilters.Controllers;

[ApiController]
[Route("api/[controller]")]
    // Áp dụng resource filter xác thực API Key cho tất cả các action trong controller này
    // Điều này đảm bảo mọi request đến bất kỳ endpoint nào ở đây đều yêu cầu một API key hợp lệ
[ApiKeyValidationResourceFilter]
    public class WeatherController : ControllerBase
    {
        // GET api/weather/forecast/{city}
        // Áp dụng resource filter caching cho action này để cache response cho mỗi thành phố
        [HttpGet("forecast/{city}")]
        [ServiceFilter(typeof(WeatherCacheResourceFilter))]
        public IActionResult GetWeatherForecast(string city)
        {
            // Mô phỏng việc lấy dữ liệu thời tiết với nhiệt độ ngẫu nhiên và thời gian hiện tại
            var weatherData = new
            {
                TemperatureC = new Random().Next(-10, 40),
                Condition = "Sunny",
                Timestamp = DateTime.UtcNow
            };

            // Gói dữ liệu thời tiết với khóa là thành phố, chuyển tên thành phố thành chữ thường để nhất quán
            var response = new
            {
                City = city.ToLower(),
                Data = weatherData
            };

            // Trả về HTTP 200 OK với đối tượng JSON response
            return Ok(response);
        }

        // GET api/weather/limited-forecast/{city}
        // Áp dụng filter giới hạn tần suất và filter caching cho action này, theo thứ tự đó
        // Giới hạn tần suất sẽ từ chối các request quá mức trước khi logic caching được gọi
        [HttpGet("limited-forecast/{city}")]
        [ServiceFilter(typeof(RateLimitResourceFilter))]
        [ServiceFilter(typeof(WeatherCacheResourceFilter))]
        public IActionResult GetLimitedWeatherForecast(string city)
        {
            // Mô phỏng việc lấy dữ liệu thời tiết với nhiệt độ ngẫu nhiên và thời gian hiện tại
            var weatherData = new
            {
                TemperatureC = new Random().Next(-10, 40),
                Condition = "Cloudy",
                Timestamp = DateTime.UtcNow
            };

            // Gói dữ liệu thời tiết với khóa là thành phố, chuyển tên thành phố thành chữ thường
            var response = new
            {
                City = city.ToLower(),
                Data = weatherData
            };

            // Trả về HTTP 200 OK với đối tượng JSON response
            return Ok(response);
        }

        // GET api/weather/open/{city}
        // Action này chỉ có xác thực API key từ cấp controller, không có caching hay giới hạn tần suất
        [HttpGet("open/{city}")]
        public IActionResult GetOpenWeather(string city)
        {
            // Mô phỏng việc lấy dữ liệu thời tiết với nhiệt độ ngẫu nhiên và thời gian hiện tại
            var weatherData = new
            {
                TemperatureC = new Random().Next(-10, 40),
                Condition = "Partly Cloudy",
                Timestamp = DateTime.UtcNow
            };

            // Gói dữ liệu thời tiết với khóa là thành phố, chuyển tên thành phố thành chữ thường
            var response = new
            {
                City = city.ToLower(),
                Data = weatherData
            };

            // Trả về HTTP 200 OK với đối tượng JSON response
            return Ok(response);
        }
    }