using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cấu hình host để sử dụng Serilog làm provider cho logging, trước khi build host.
// Thiết lập này sẽ đọc toàn bộ cấu hình Serilog từ file appsettings.json.

// context: Cung cấp thông tin cấu hình của ứng dụng (ví dụ: biến môi trường, appsettings.json,...).
// services: IServiceProvider dùng để inject các dịch vụ trong ứng dụng.
// configuration: Đối tượng LoggerConfiguration dùng để cấu hình Serilog.
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
    configuration.ReadFrom.Services(services);

    // Bọc các sink bằng Async trong code
    configuration.WriteTo.Async(a =>
    {
        a.Console();
        a.File("logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30);
    });
});

 // Cấu hình Serilog:
// - Đọc cấu hình từ appsettings.json thông qua builder.Configuration
// - Ghi log ra console và file
Log.Logger = new LoggerConfiguration()             // Tạo cấu hình logger mới
                .ReadFrom.Configuration(builder.Configuration) // Đọc thiết lập từ appsettings.json
                .WriteTo.Console()                             // Ghi log ra console
                .WriteTo.File("logs/MyAppLog.txt")             // Ghi log ra file
                .CreateLogger();

 // Thay thế provider logging mặc định bằng Serilog
builder.Host.UseSerilog(); // Thiết lập Serilog làm provider logging chính

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
