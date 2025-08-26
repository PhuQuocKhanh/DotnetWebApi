using Microsoft.EntityFrameworkCore;
using ValidateNestedComplex.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                // Tùy chọn, cấu hình các tùy chọn JSON hoặc các thiết lập định dạng khác
                .AddJsonOptions(options =>
                {
                    // Cấu hình serializer JSON để giữ nguyên tên gốc trong quá trình serialization và deserialization
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

builder.Services.AddDbContext<ECommerceDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ECommerceDBConnection")));

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
