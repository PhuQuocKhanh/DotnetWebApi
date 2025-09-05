using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace BasicAuthConsoleClient
{
    public class Program
    {
        private static readonly HttpClient client = new HttpClient();
        // Thay đổi các giá trị này theo API và thông tin xác thực của bạn
        private const string BaseUrl = "http://localhost:5298/api/product";  
        private const string Username = "john.doe@example.com";
        private const string Password = "John@123";

        public static async Task Main(string[] args)
        {
            // Thiết lập Header Basic Auth
            var authToken = Encoding.ASCII.GetBytes($"{Username}:{Password}");
            var base64AuthToken = Convert.ToBase64String(authToken);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64AuthToken);

            Console.WriteLine("Enter Product Id to GET:");
            if (int.TryParse(Console.ReadLine(), out int productId))
            {
                await GetProductByIdAsync(productId);
            }
            else
            {
                Console.WriteLine("Invalid product Id input.");
            }

            Console.WriteLine("\nCreating new product...");
            await CreateProductAsync(new CreateProductDTO
            {
                Name = "Console App Product",
                Description = "Created via Console App",
                Price = 123.45m,
                Stock = 10
            });

            Console.ReadLine();
        }

        private static async Task GetProductByIdAsync(int id)
        {
            var response = await client.GetAsync($"{BaseUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Product {id} details: {json}");
            }
            else
            {
                Console.WriteLine($"Failed to get product. Status: {response.StatusCode}");
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error details: {error}");
            }
        }

        private static async Task CreateProductAsync(CreateProductDTO product)
        {
            var jsonContent = JsonSerializer.Serialize(product);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(BaseUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Created Product: {json}");
            }
            else
            {
                Console.WriteLine($"Failed to create product. Status: {response.StatusCode}");
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error details: {error}");
            }
        }
    }

    // DTO khớp với CreateProductDTO của API
    public class CreateProductDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}