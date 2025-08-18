using MemoryCacheDemo.CustomInMemoryCache.Service;
using MemoryCacheDemo.InMemoryCache;
using MemoryCacheDemo.InMemoryCache.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
 // Đăng ký ApplicationDbContext với chuỗi kết nối SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký dịch vụ in-memory caching để lưu dữ liệu trong RAM
builder.Services.AddMemoryCache();

builder.Services.AddScoped<LocationRepository>();

builder.Services.AddSingleton<CacheManager>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
