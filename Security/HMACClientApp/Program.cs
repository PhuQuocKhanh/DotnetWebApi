// Client ID, Secret Key, và Base URL của API
using HMACClientApp;

var clientId = "DesktopClient";
var secretKey = "m1n2b3v4c5x6z7l8k9j0";
var baseUrl = "https://localhost:7035"; // Thay đổi port nếu cần
var client = new HttpClient
{
    Timeout = TimeSpan.FromMinutes(5)
};

try
{
    // Tạo một Nhân viên Mới (Yêu cầu POST)
    var employee = new { Name = "Pranaya Rout", Position = "Developer", Salary = 60000 };
    var response = await HMACHelper.SendRequestAsync(client, HttpMethod.Post, baseUrl, "/api/employees", clientId, secretKey, employee);
    if (response.IsSuccessStatusCode)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine("POST Response: Employee Created Successfully");
        Console.WriteLine($"Response Content: {responseContent}");
    }
    else
    {
        Console.WriteLine($"POST Error: {response.StatusCode} - {response.ReasonPhrase}");
    }

    // Lấy tất cả Nhân viên (Yêu cầu GET)
    response = await HMACHelper.SendRequestAsync(client, HttpMethod.Get, baseUrl, "/api/employees", clientId, secretKey);
    if (response.IsSuccessStatusCode)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine("\nGET Response: Employees Retrieved Successfully");
        Console.WriteLine($"Response Content: {responseContent}");
    }
    else
    {
        Console.WriteLine($"GET Error: {response.StatusCode} - {response.ReasonPhrase}");
    }
    
    // Cập nhật Nhân viên (Yêu cầu PUT)
    var employeeId = 1;
    var updatedEmployee = new { Id = employeeId, Name = "Rakesh Sharma", Position = "Senior Developer", Salary = 80000 };
    response = await HMACHelper.SendRequestAsync(client, HttpMethod.Put, baseUrl, $"/api/employees/{employeeId}", clientId, secretKey, updatedEmployee);
    if (response.IsSuccessStatusCode)
    {
        Console.WriteLine($"\nPUT Response: Employee Updated Successfully. Status: {response.StatusCode}");
    }
    else
    {
        Console.WriteLine($"\nPUT Error: {response.StatusCode} - {response.ReasonPhrase}");
    }

    // Xóa Nhân viên (Yêu cầu DELETE)
    response = await HMACHelper.SendRequestAsync(client, HttpMethod.Delete, baseUrl, $"/api/employees/{employeeId}", clientId, secretKey);
    if (response.IsSuccessStatusCode)
    {
        Console.WriteLine($"\nDELETE Response: Employee Deleted Successfully. Status: {response.StatusCode}");
    }
    else
    {
            Console.WriteLine($"\nDELETE Error: {response.StatusCode} - {response.ReasonPhrase}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected Error: {ex.Message}");
}
Console.ReadKey();