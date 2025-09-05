using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using UserClientApp.Models;

namespace UserClientApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    //The Index action method will load the list of users from the API
    //and pass it to the view on the initial load.
    public async Task<IActionResult> Index()
    {
        //Create an Instance of HttpClient
        HttpClient _httpClient = new HttpClient();
        //Set the base Address
        //Please replace the port on which your Web API Application is running
        _httpClient.BaseAddress = new Uri("http://localhost:5159/");
        //When the Page Load we want to display the List of User
        //Specify the endpoint which returns the list of Users, i.e., api/User
        //Here, we will not get any CORS issue, this is because of Server to Server call
        var response = await _httpClient.GetStringAsync("api/User");
        //Convert the Response which is in JSON format to List<User>
        var users = JsonSerializer.Deserialize<List<User>>(response);
        //Pass the List of Users to the View
        return View(users);
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
