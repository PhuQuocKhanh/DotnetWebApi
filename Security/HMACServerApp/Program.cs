using HMACServerApp.Middlewares;
using HMACServerApp.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Sử dụng tên thuộc tính như được định nghĩa trong model C#
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<HMACDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("EFCoreDBConnection"));
            });

// Thêm Caching trong bộ nhớ
builder.Services.AddMemoryCache();
// Đăng ký ClientSecretService
builder.Services.AddScoped<ClientSecretService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Đăng ký HMACAuthenticationMiddleware
app.UseMiddleware<HMACAuthenticationMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
