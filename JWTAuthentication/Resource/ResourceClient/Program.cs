using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ResourceClient.DTOs;

namespace ResourceClient;

class Program
{
    // Configuration settings
    private static readonly string AuthServerBaseUrl = "http://localhost:5242"; // Authentication Server URL
    private static readonly string ResourceServerBaseUrl = "http://localhost:5073"; // Replace with your Resource Server's URL and port
    private static readonly string ClientId = "Client1"; // Must match a valid ClientId in Auth Server
    private static readonly string UserEmail = "john.doe@example.com"; // Replace with registered user's email
    private static readonly string UserPassword = "Password@123"; // Replace with registered user's password
    
    // Instance lưu trữ token
    private static readonly TokenStorage tokenStorage = new TokenStorage();

    // Instance HttpClient (được chia sẻ)
    private static readonly HttpClient httpClient = new HttpClient();

    static async Task Main(string[] args)
    {
        try
        {
            // Bước 1: Xác thực và lấy JWT token cùng Refresh Token
            var loginSuccess = await AuthenticateAsync(UserEmail, UserPassword, ClientId);
            if (!loginSuccess)
            {
                Console.WriteLine("Xác thực thất bại. Đang thoát...");
                return;
            }
            Console.WriteLine("Xác thực thành công. Đã lấy được token.\n");

            // Bước 2: Sử dụng các endpoint của ProductsController trên máy chủ tài nguyên
            await ConsumeResourceServerAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Đã xảy ra lỗi: {ex.Message}");
        }
        finally
        {
            httpClient.Dispose();
        }
    }

    // Xác thực người dùng với máy chủ xác thực và lấy JWT và Refresh token.
    private static async Task<bool> AuthenticateAsync(string email, string password, string clientId)
    {
        var loginUrl = $"{AuthServerBaseUrl}/api/Auth/Login";
        var loginData = new
        {
            Email = email,
            Password = password,
            ClientId = clientId
        };

        var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");
        Console.WriteLine("Đang gửi request xác thực...");
        var response = await httpClient.PostAsync(loginUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Xác thực thất bại với mã trạng thái: {response.StatusCode}");
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Lỗi: {errorContent}\n");
            return false;
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var loginResponse = JsonSerializer.Deserialize<LoginResponseDTO>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token) && !string.IsNullOrEmpty(loginResponse.RefreshToken))
        {
            tokenStorage.AccessToken = loginResponse.Token;
            tokenStorage.RefreshToken = loginResponse.RefreshToken;
            tokenStorage.ClientId = clientId;
            return true;
        }

        Console.WriteLine("Không tìm thấy token trong response xác thực.\n");
        return false;
    }

    // Sử dụng các endpoint của ProductsController trên máy chủ tài nguyên bằng JWT token.
    private static async Task ConsumeResourceServerAsync()
    {
        // Đặt header Authorization với Bearer token
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenStorage.AccessToken);

        // Tạo một sản phẩm mới
        var newProduct = new
        {
            Name = "Smartphone",
            Description = "Một chiếc smartphone cao cấp với các tính năng tuyệt vời.",
            Price = 999.99
        };
        Console.WriteLine("Đang tạo một sản phẩm mới...");
        var createResponse = await httpClient.PostAsync($"{ResourceServerBaseUrl}/api/Products/Add",
            new StringContent(JsonSerializer.Serialize(newProduct), Encoding.UTF8, "application/json"));

        if (createResponse.IsSuccessStatusCode)
        {
            var createdProductJson = await createResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Sản phẩm đã được tạo thành công: {createdProductJson}\n");
        }
        else if (createResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            Console.WriteLine("Access token đã hết hạn hoặc không hợp lệ. Đang cố gắng refresh token...");
            var refreshSuccess = await RefreshTokenAsync();
            if (refreshSuccess)
            {
                // Thử lại request với token mới
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenStorage.AccessToken);
                createResponse = await httpClient.PostAsync($"{ResourceServerBaseUrl}/api/Products/Add",
                    new StringContent(JsonSerializer.Serialize(newProduct), Encoding.UTF8, "application/json"));
                if (createResponse.IsSuccessStatusCode)
                {
                    var createdProductJson = await createResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Sản phẩm đã được tạo thành công sau khi refresh token: {createdProductJson}\n");
                }
                else
                {
                    Console.WriteLine($"Thất bại khi tạo sản phẩm sau khi refresh token. Mã trạng thái: {createResponse.StatusCode}");
                    var errorContent = await createResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Lỗi: {errorContent}\n");
                }
            }
            else
            {
                Console.WriteLine("Thất bại khi refresh token. Đang thoát...");
                return;
            }
        }
        else
        {
            Console.WriteLine($"Thất bại khi tạo sản phẩm. Mã trạng thái: {createResponse.StatusCode}");
            var errorContent = await createResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Lỗi: {errorContent}\n");
        }

        // Lấy tất cả các sản phẩm
        Console.WriteLine("Đang lấy tất cả sản phẩm...");
        var getAllResponse = await httpClient.GetAsync($"{ResourceServerBaseUrl}/api/Products/GetAll");
        if (getAllResponse.IsSuccessStatusCode)
        {
            var productsJson = await getAllResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Sản phẩm: {productsJson}\n");
        }
        else if (getAllResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            Console.WriteLine("Access token đã hết hạn hoặc không hợp lệ. Đang cố gắng refresh token...");
            var refreshSuccess = await RefreshTokenAsync();
            if (refreshSuccess)
            {
                // Thử lại request với token mới
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenStorage.AccessToken);
                getAllResponse = await httpClient.GetAsync($"{ResourceServerBaseUrl}/api/Products/GetAll");
                if (getAllResponse.IsSuccessStatusCode)
                {
                    var productsJson = await getAllResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Sản phẩm sau khi refresh token: {productsJson}\n");
                }
                else
                {
                    Console.WriteLine($"Thất bại khi lấy sản phẩm sau khi refresh token. Mã trạng thái: {getAllResponse.StatusCode}");
                    var errorContent = await getAllResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Lỗi: {errorContent}\n");
                }
            }
            else
            {
                Console.WriteLine("Thất bại khi refresh token. Đang thoát...");
                return;
            }
        }
        else
        {
            Console.WriteLine($"Thất bại khi lấy sản phẩm. Mã trạng thái: {getAllResponse.StatusCode}");
            var errorContent = await getAllResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Lỗi: {errorContent}\n");
        }

        // Lấy một sản phẩm cụ thể theo ID
        Console.WriteLine("Đang lấy sản phẩm có ID 1...");
        var getByIdResponse = await httpClient.GetAsync($"{ResourceServerBaseUrl}/api/Products/GetById/1");
        if (getByIdResponse.IsSuccessStatusCode)
        {
            var productJson = await getByIdResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Chi tiết sản phẩm: {productJson}\n");
        }
        else if (getByIdResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            Console.WriteLine("Access token đã hết hạn hoặc không hợp lệ. Đang cố gắng refresh token...");
            var refreshSuccess = await RefreshTokenAsync();
            if (refreshSuccess)
            {
                // Thử lại request với token mới
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenStorage.AccessToken);
                getByIdResponse = await httpClient.GetAsync($"{ResourceServerBaseUrl}/api/Products/GetById/1");
                if (getByIdResponse.IsSuccessStatusCode)
                {
                    var productJson = await getByIdResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Chi tiết sản phẩm sau khi refresh token: {productJson}\n");
                }
                else
                {
                    Console.WriteLine($"Thất bại khi lấy sản phẩm sau khi refresh token. Mã trạng thái: {getByIdResponse.StatusCode}");
                    var errorContent = await getByIdResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Lỗi: {errorContent}\n");
                }
            }
            else
            {
                Console.WriteLine("Thất bại khi refresh token. Đang thoát...");
                return;
            }
        }
        else
        {
            Console.WriteLine($"Thất bại khi lấy sản phẩm. Mã trạng thái: {getByIdResponse.StatusCode}");
            var errorContent = await getByIdResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Lỗi: {errorContent}\n");
        }

        // Cập nhật một sản phẩm
        var updatedProduct = new
        {
            Name = "Smartphone Pro",
            Description = "Một chiếc smartphone nâng cấp với các tính năng tăng cường.",
            Price = 1199.99
        };
        Console.WriteLine("Đang cập nhật sản phẩm có ID 1...");
        var updateResponse = await httpClient.PutAsync($"{ResourceServerBaseUrl}/api/Products/Update/1",
            new StringContent(JsonSerializer.Serialize(updatedProduct), Encoding.UTF8, "application/json"));
        if (updateResponse.IsSuccessStatusCode || updateResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            Console.WriteLine("Sản phẩm đã được cập nhật thành công.\n");
        }
        else if (updateResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            Console.WriteLine("Access token đã hết hạn hoặc không hợp lệ. Đang cố gắng refresh token...");
            var refreshSuccess = await RefreshTokenAsync();
            if (refreshSuccess)
            {
                // Thử lại request với token mới
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenStorage.AccessToken);
                updateResponse = await httpClient.PutAsync($"{ResourceServerBaseUrl}/api/Products/Update/1",
                    new StringContent(JsonSerializer.Serialize(updatedProduct), Encoding.UTF8, "application/json"));
                if (updateResponse.IsSuccessStatusCode || updateResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    Console.WriteLine("Sản phẩm đã được cập nhật thành công sau khi refresh token.\n");
                }
                else
                {
                    Console.WriteLine($"Thất bại khi cập nhật sản phẩm sau khi refresh token. Mã trạng thái: {updateResponse.StatusCode}");
                    var errorContent = await updateResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Lỗi: {errorContent}\n");
                }
            }
            else
            {
                Console.WriteLine("Thất bại khi refresh token. Đang thoát...");
                return;
            }
        }
        else
        {
            Console.WriteLine($"Thất bại khi cập nhật sản phẩm. Mã trạng thái: {updateResponse.StatusCode}");
            var errorContent = await updateResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Lỗi: {errorContent}\n");
        }

        // Xóa một sản phẩm
        Console.WriteLine("Đang xóa sản phẩm có ID 1...");
        var deleteResponse = await httpClient.DeleteAsync($"{ResourceServerBaseUrl}/api/Products/Delete/1");
        if (deleteResponse.IsSuccessStatusCode || deleteResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            Console.WriteLine("Sản phẩm đã được xóa thành công.\n");
        }
        else if (deleteResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            Console.WriteLine("Access token đã hết hạn hoặc không hợp lệ. Đang cố gắng refresh token...");
            var refreshSuccess = await RefreshTokenAsync();
            if (refreshSuccess)
            {
                // Thử lại request với token mới
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenStorage.AccessToken);
                deleteResponse = await httpClient.DeleteAsync($"{ResourceServerBaseUrl}/api/Products/Delete/1");
                if (deleteResponse.IsSuccessStatusCode || deleteResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    Console.WriteLine("Sản phẩm đã được xóa thành công sau khi refresh token.\n");
                }
                else
                {
                    Console.WriteLine($"Thất bại khi xóa sản phẩm sau khi refresh token. Mã trạng thái: {deleteResponse.StatusCode}");
                    var errorContent = await deleteResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Lỗi: {errorContent}\n");
                }
            }
            else
            {
                Console.WriteLine("Thất bại khi refresh token. Đang thoát...");
                return;
            }
        }
        else
        {
            Console.WriteLine($"Thất bại khi xóa sản phẩm. Mã trạng thái: {deleteResponse.StatusCode}");
            var errorContent = await deleteResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Lỗi: {errorContent}\n");
        }
    }

    // Refreshes access token bằng refresh token
    private static async Task<bool> RefreshTokenAsync()
    {
        var refreshUrl = $"{AuthServerBaseUrl}/api/Auth/RefreshToken";
        var refreshData = new RefreshTokenRequestDTO
        {
            RefreshToken = tokenStorage.RefreshToken,
            ClientId = tokenStorage.ClientId
        };

        var content = new StringContent(JsonSerializer.Serialize(refreshData), Encoding.UTF8, "application/json");
        Console.WriteLine("Đang cố gắng refresh access token...");
        var response = await httpClient.PostAsync(refreshUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Refresh token thất bại với mã trạng thái: {response.StatusCode}");
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Lỗi: {errorContent}\n");
            return false;
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var refreshResponse = JsonSerializer.Deserialize<RefreshTokenResponseDTO>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (refreshResponse != null && !string.IsNullOrEmpty(refreshResponse.Token) && !string.IsNullOrEmpty(refreshResponse.RefreshToken))
        {
            tokenStorage.AccessToken = refreshResponse.Token;
            tokenStorage.RefreshToken = refreshResponse.RefreshToken;
            Console.WriteLine("Access token đã được refresh thành công.\n");
            return true;
        }

        Console.WriteLine("Thất bại khi phân tích response refresh token.\n");
        return false;
    }
}
