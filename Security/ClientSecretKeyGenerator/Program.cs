using ClientSecretKeyGenerator.DTOs;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ClientSecretKeyGenerator
{
    internal class Program
    {
        // Thay đổi URL cơ sở này theo thiết lập API của bạn
        private static readonly string baseUrl = "http://localhost:5197";
        private static readonly string clientId = "client-app-one";

        private static string accessToken = string.Empty;
        private static string refreshToken = string.Empty;

        static async Task Main(string[] args)
        {
            // 1. Đăng nhập để lấy JWT + Refresh Token
            Console.WriteLine("Logging in...");
            var auth = await LoginAsync("pranaya.rout@example.com", "Password123!", clientId);
            if (auth == null)
            {
                Console.WriteLine("Login failed!");
                return;
            }

            accessToken = auth.AccessToken;
            refreshToken = auth.RefreshToken;
            Console.WriteLine("Login successful!");
            Console.WriteLine($"Access Token: {accessToken.Substring(0, 20)}...");
            Console.WriteLine($"Refresh Token: {refreshToken.Substring(0, 20)}...");

            // 2. Gọi endpoint được bảo vệ với JWT
            await CallProtectedApiAsync();

            // 3. Giả lập token hết hạn: xóa JWT thủ công để kiểm tra luồng làm mới (dành cho demo)
            Console.WriteLine("\nSimulating token expiry...");
            accessToken = "InvalidOrExpiredToken";

            // 4. Thử gọi lại endpoint được bảo vệ, mong đợi lỗi 401
            await CallProtectedApiAsync();

            // 5. Sử dụng Refresh Token để lấy JWT mới
            Console.WriteLine("\nRefreshing Access Token...");
            var newAuth = await RefreshTokenAsync(refreshToken, clientId);
            if (newAuth == null)
            {
                Console.WriteLine("Refresh token failed!");
                return;
            }

            accessToken = newAuth.AccessToken;
            refreshToken = newAuth.RefreshToken;
            Console.WriteLine("Access Token refreshed!");

            // 6. Thử gọi lại endpoint được bảo vệ với token mới
            await CallProtectedApiAsync();

            Console.WriteLine("\nDemo completed");
            Console.ReadKey();
        }

        static async Task<AuthResponse?> LoginAsync(string email, string password, string clientId)
        {
            using (var client = new HttpClient())
            {
                var loginReq = new
                {
                    Email = email,
                    Password = password,
                    ClientId = clientId
                };
                var content = new StringContent(JsonSerializer.Serialize(loginReq), Encoding.UTF8, "application/json");
                var resp = await client.PostAsync($"{baseUrl}/api/Auth/login", content);
                if (!resp.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Login failed: {resp.StatusCode}");
                    return null;
                }
                var body = await resp.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<AuthResponse>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }

        static async Task CallProtectedApiAsync()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var resp = await client.GetAsync($"{baseUrl}/api/Protected/userdata");

                if (resp.IsSuccessStatusCode)
                {
                    var body = await resp.Content.ReadAsStringAsync();
                    Console.WriteLine("\n[Protected API Response]:");
                    Console.WriteLine(body);
                }
                else
                {
                    Console.WriteLine($"\n[Protected API] Failed: {resp.StatusCode}");
                    var body = await resp.Content.ReadAsStringAsync();
                    Console.WriteLine(body);
                }
            }
        }

        static async Task<AuthResponse?> RefreshTokenAsync(string refreshToken, string clientId)
        {
            using (var client = new HttpClient())
            {
                var req = new
                {
                    RefreshToken = refreshToken,
                    ClientId = clientId
                };
                var content = new StringContent(JsonSerializer.Serialize(req), Encoding.UTF8, "application/json");
                var resp = await client.PostAsync($"{baseUrl}/api/Auth/refresh-token", content);
                if (!resp.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Refresh token failed: {resp.StatusCode}");
                    return null;
                }
                var body = await resp.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<AuthResponse>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }
    }
}