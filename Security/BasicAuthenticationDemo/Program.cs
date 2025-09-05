using BasicAuthenticationDemo.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                // Vô hiệu hóa chính sách đặt tên camelCase cho các thuộc tính JSON
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("EFCoreDBConnection")));

// Đăng ký ProductService cho dependency injection
builder.Services.AddScoped<IProductService, ProductService>();

// Cấu hình dịch vụ Authentication để sử dụng scheme Basic Authentication.
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
// - "BasicAuthentication": Tên scheme để xác định handler này
// - AuthenticationSchemeOptions: Tùy chọn mặc định
// - BasicAuthenticationHandler: Handler tùy chỉnh của chúng ta
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
