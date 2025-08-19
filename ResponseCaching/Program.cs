using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResponseCaching.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    // Tạo Custom Cache Profile
    // Profile "Default60":
    // cache trong 60 giây và cho phép cache ở mọi nơi
    options.CacheProfiles.Add("Default60", new CacheProfile()
    {
        Duration = 60,
        Location = ResponseCacheLocation.Any
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Đăng ký DbContext với SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký dịch vụ Response Caching
builder.Services.AddResponseCaching();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Thêm middleware Response Caching vào pipeline
app.UseResponseCaching();

app.MapControllers();

app.Run();
