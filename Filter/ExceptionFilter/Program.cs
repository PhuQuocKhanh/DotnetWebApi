using ExceptionFilter.Filters;
using ExceptionFilter.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Thêm các dịch vụ vào container.
builder.Services.AddControllers(options =>
{
    // Đăng ký Exception Filter một cách toàn cục
    //options.Filters.Add<CustomExceptionFilter>();
})
.AddJsonOptions(options =>
{
    // Vô hiệu hóa camelCase trong đầu ra JSON, giữ nguyên tên thuộc tính như đã định nghĩa trong các class C#
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});
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

// Đăng ký Custom Exception Handling Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
