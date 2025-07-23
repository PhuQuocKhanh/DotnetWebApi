
using Microsoft.OpenApi.Models;
using MyFirstWebAPIProject.Filter;
using MyFirstWebAPIProject.Middlewares;
using MyFirstWebAPIProject.Policy;
using MyFirstWebAPIProject.Repositories;
using MyFirstWebAPIProject.Services;

namespace MyFirstWebAPIProject;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v10", new OpenApiInfo
            {
                Title = "My Custom API",
                Version = "v10",
                Description = "A Brief Description of My APIs",
                TermsOfService = new Uri("https://dotnettutorials.net/privacy-policy/"),
                Contact = new OpenApiContact
                {
                    Name = "Support",
                    Email = "support@dotnettutorials.net",
                    Url = new Uri("https://dotnettutorials.net/contact/")
                },
                License = new OpenApiLicense
                {
                    Name = "Use Under XYZ",
                    Url = new Uri("https://dotnettutorials.net/about-us/")
                }
            });
        });
        //Adding Response Caching Services
        builder.Services.AddResponseCaching();

        builder.Services.AddHttpClient();
        // Đăng ký HttpClient kết hợp với Polly
        // Gán HttpClient cho interface IMyService với implementation là MyService
        // builder.Services.AddHttpClient<IRetryPolly, RetryPolly>()
        // // Áp dụng handler sử dụng chính sách retry từ Polly
        //                 .AddPolicyHandler(PollyPolicies.GetRetryPolicy());

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v10/swagger.json", "My API V10");
            });        
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        // app.UseMiddleware<CustomAuthenticationMiddleware>();
        // app.UseMiddleware<CustomAuthentication403Middleware>();
        // app.UseMiddleware<NotFoundCustomMiddleware>();
        // app.UseMiddleware<CustomAuthentication405MethodNotAllowMiddleware>();
        // app.UseMiddleware<ErrorHandlingMiddleware500>();
        // app.UseMiddleware<MaintenanceMiddleware503>();
        app.UseMiddleware<GatewayTimeoutMiddleware>();
        
        // var allowedMethods = new[] { "GET", "POST", "DELETE" };
        // app.Use(async (context, next) =>
        // {
        //     var middleware = new HttpMethodMiddleware(next, allowedMethods);
        //     await middleware.InvokeAsync(context);
        // });

        // builder.Services.AddControllers(options =>
        // {
        //     options.Filters.Add(new HttpMethodFilter(["GET", "POST", "DELETE"]));
        // });

        app.MapControllers();

        app.Run();
    }
}
