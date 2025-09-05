using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HMACClientApp
{
    public class HMACHelper
    {
        // Phương thức để tạo token HMAC
        public static string GenerateHmacToken(string method, string path, string clientId, string secretKey, string requestBody = "")
        {
            // Tạo một nonce duy nhất
            var nonce = Guid.NewGuid().ToString();
            // Lấy timestamp UTC hiện tại dưới dạng Unix timestamp
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

            // Xây dựng nội dung yêu cầu
            var requestContent = new StringBuilder()
                .Append(method.ToUpper())
                .Append(path.ToUpper())
                .Append(nonce)
                .Append(timestamp);

            // Nếu là POST hoặc PUT, nối thêm request body
            if (method == HttpMethod.Post.Method || method == HttpMethod.Put.Method)
            {
                requestContent.Append(requestBody);
            }

            // Chuyển đổi khóa bí mật và nội dung yêu cầu thành byte
            var secretBytes = Encoding.UTF8.GetBytes(secretKey);
            var requestBytes = Encoding.UTF8.GetBytes(requestContent.ToString());

            // Tạo instance HMACSHA256
            using var hmac = new HMACSHA256(secretBytes);
            // Tính toán hash
            var computedHash = hmac.ComputeHash(requestBytes);
            // Chuyển đổi hash thành chuỗi base64
            var computedToken = Convert.ToBase64String(computedHash);

            // Nối clientId, token, nonce, và timestamp để tạo token cuối cùng
            return $"{clientId}|{computedToken}|{nonce}|{timestamp}";
        }

        // Phương thức hỗ trợ để gửi yêu cầu API
        public static async Task<HttpResponseMessage> SendRequestAsync(
            HttpClient client,
            HttpMethod method,
            string baseUrl,
            string endpoint,
            string clientId,
            string secretKey,
            object? data = null)
        {
            // Serialize dữ liệu thành JSON nếu có
            var requestBody = data != null ? System.Text.Json.JsonSerializer.Serialize(data) : string.Empty;
            
            // Tạo token HMAC để xác thực
            var token = HMACHelper.GenerateHmacToken(method.Method, endpoint, clientId, secretKey, requestBody);

            // Xây dựng yêu cầu HTTP
            var requestMessage = new HttpRequestMessage(method, $"{baseUrl}{endpoint}")
            {
                Content = !string.IsNullOrEmpty(requestBody)
                    ? new StringContent(requestBody, Encoding.UTF8, "application/json")
                    : null
            };

            // Thêm header Authorization với token HMAC
            requestMessage.Headers.Add("Authorization", $"HMAC {token}");
            
            // Gửi yêu cầu và trả về phản hồi
            return await client.SendAsync(requestMessage);
        }
    }
}