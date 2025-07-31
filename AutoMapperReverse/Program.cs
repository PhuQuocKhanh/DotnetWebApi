using AutoMapperReverse.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

  // Đăng ký AutoMapper và cấu hình Profile
            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            // Đăng ký dịch vụ Controller và tùy chỉnh JSON nếu cần
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Giữ nguyên tên thuộc tính gốc khi serialize/deserialize
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

            // Đăng ký DbContext sử dụng chuỗi kết nối
            builder.Services.AddDbContext<EmployeeDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeDBConnection")));
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
