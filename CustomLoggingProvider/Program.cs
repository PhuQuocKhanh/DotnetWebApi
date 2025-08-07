using CustomLoggingProvider.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<LoggingDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("LoggingDBConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
string logFilePath = "Logs/logs.txt"; // Đường dẫn nơi file log sẽ được ghi

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>(); // Lấy IServiceScopeFactory

var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>(); // Lấy ILoggerFactory mặc định
loggerFactory.AddProvider(new CustomLoggerProvider(logFilePath, LogLevel.Information, scopeFactory)); // Thêm Custom Logger Provider

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
