using Serilog;

// Khởi tạo Serilog sớm để bắt log khởi động
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Preserve original property names during JSON serialization/deserialization.
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
// Replace default logging with Serilog
// Configure Serilog using the settings from appsettings.json
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.WebHost.UseUrls("http://localhost:5000");


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

// Ghi log thử để kiểm tra Serilog gửi lên Elasticsearch
Log.Information("Test log to Elasticsearch at {Time}", DateTime.UtcNow);

app.Run();

// Đảm bảo flush log khi app kết thúc
Log.CloseAndFlush();
