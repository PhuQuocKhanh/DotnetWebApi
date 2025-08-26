using ResourceFilters.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

 builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                // Vô hiệu hóa camelCase trong đầu ra JSON, giữ nguyên tên thuộc tính như định nghĩa trong lớp C#
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
// Đăng ký MemoryCache
builder.Services.AddMemoryCache();

// Không cần đăng ký ResourceFilter dạng Attribute ở đây
builder.Services.AddScoped<WeatherCacheResourceFilter>();
builder.Services.AddScoped<RateLimitResourceFilter>();

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
