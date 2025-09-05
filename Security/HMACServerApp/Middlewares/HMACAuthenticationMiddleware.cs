using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HMACServerApp.Models;
using Microsoft.Extensions.Caching.Memory;

namespace HMACServerApp.Middlewares
{
    public class HMACAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        // Thời gian hết hạn của Nonce để ngăn chặn việc sử dụng lại
        private static readonly TimeSpan NonceExpiry = TimeSpan.FromMinutes(5);

        // Constructor để khởi tạo middleware
        public HMACAuthenticationMiddleware(RequestDelegate next, IMemoryCache memoryCache, IConfiguration configuration)
        {
            _next = next;
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        // Xử lý mỗi request và thực hiện xác thực HMAC
        public async Task Invoke(HttpContext context)
        {
            // Kiểm tra xem HMAC có được bật không
            var isHMACEnabled = _configuration.GetValue<bool>("HMACSettings:EnableHMAC");
            if (!isHMACEnabled)
            {
                // Bỏ qua xác thực HMAC và gọi middleware tiếp theo
                await _next(context);
                return;
            }

            // Tiếp tục xác thực HMAC
            if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Authorization header missing");
                return;
            }

            if (!authHeader.ToString().StartsWith("HMAC ", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid Authorization header");
                return;
            }

            // Trích xuất các phần của token từ header
            var tokenParts = authHeader.ToString().Substring("HMAC ".Length).Trim().Split('|');
            if (tokenParts.Length != 4)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid HMAC format");
                return;
            }
            var clientId = tokenParts[0];
            var token = tokenParts[1];
            var nonce = tokenParts[2];
            var timestamp = tokenParts[3];

            // Lấy instance ClientSecretService từ DI
            var clientSecretService = context.RequestServices.GetRequiredService<ClientSecretService>();
            
            // Xác thực client ID và lấy khóa bí mật
            var secretKey = await clientSecretService.GetSecretKeyAsync(clientId);
            if (string.IsNullOrEmpty(secretKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid client ID");
                return;
            }

            // Xác thực timestamp
            if (!long.TryParse(timestamp, out var timestampSeconds))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid timestamp format");
                return;
            }

            // Chuyển đổi timestamp thành Unix Time
            var requestTime = DateTimeOffset.FromUnixTimeSeconds(timestampSeconds).UtcDateTime;
            var currentTime = DateTime.UtcNow;

            // Kiểm tra xem timestamp có nằm trong khoảng thời gian cho phép không (trong vòng 5 phút)
            // để tránh Tấn công phát lại (Replay Attack)
            if (Math.Abs((currentTime - requestTime).TotalMinutes) > 5)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Timestamp is outside the allowable range");
                return;
            }

            // Xác thực nonce sử dụng key cache dành riêng cho client
            var nonceKey = $"{clientId}:{nonce}";
            if (_memoryCache.TryGetValue(nonceKey, out _))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Nonce has already been used");
                return;
            }
            
            // Thêm nonce vào cache với thời gian hết hạn
            _memoryCache.Set(nonceKey, true, NonceExpiry);

            // Đọc request body cho các request POST và PUT
            var requestBody = string.Empty;
            if (context.Request.Method == HttpMethod.Post.Method || context.Request.Method == HttpMethod.Put.Method)
            {
                // context.Request.EnableBuffering() cho phép đọc body của request nhiều lần.
                // Điều này cần thiết vì body cần được đọc để tính HMAC, nhưng cũng phải có sẵn
                // cho các middleware hoặc controller phía sau.
                context.Request.EnableBuffering();
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
                {
                    requestBody = await reader.ReadToEndAsync();
                    // Đặt lại vị trí của stream về đầu để nó có thể được đọc lại.
                    context.Request.Body.Position = 0;
                }
            }
            
            // Xác thực token HMAC
            var isValid = ValidateToken(token, nonce, timestamp, context.Request, requestBody, secretKey);
            if (!isValid)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid HMAC token");
                return;
            }

            // Gọi middleware tiếp theo trong pipeline
            await _next(context);
        }

        // Phương thức để xác thực token HMAC
        private bool ValidateToken(string token, string nonce, string timestamp, HttpRequest request, string requestBody, string secretKey)
        {
            var path = Convert.ToString(request.Path);
            // Xây dựng nội dung yêu cầu bằng cách nối phương thức, đường dẫn, nonce, và timestamp
            var requestContent = new StringBuilder()
                .Append(request.Method.ToUpper())
                .Append(path.ToUpper())
                .Append(nonce)
                .Append(timestamp);

            // Bao gồm request body cho các phương thức POST và PUT
            if (request.Method == HttpMethod.Post.Method || request.Method == HttpMethod.Put.Method)
            {
                requestContent.Append(requestBody);
            }
            
            // Chuyển đổi khóa bí mật và nội dung yêu cầu sang byte
            var secretBytes = Encoding.UTF8.GetBytes(secretKey);
            var requestBytes = Encoding.UTF8.GetBytes(requestContent.ToString());
            
            // Tạo instance HMACSHA256 với khóa bí mật
            using var hmac = new HMACSHA256(secretBytes);
            // Tính toán hash của nội dung yêu cầu
            var computedHash = hmac.ComputeHash(requestBytes);
            // Chuyển đổi hash đã tính toán thành chuỗi base64 (token)
            var computedToken = Convert.ToBase64String(computedHash);

            // So sánh HMAC đã tạo với HMAC nhận được từ yêu cầu
            return token == computedToken;
        }
    }
}