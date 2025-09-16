using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Cấu hình Swagger
builder.Services.AddSwaggerGen(options =>
{
    // Tạo document riêng cho từng version
    options.SwaggerDoc("1.0", new OpenApiInfo { Title = "My API", Version = "1.0" });
    options.SwaggerDoc("2.0", new OpenApiInfo { Title = "My API", Version = "2.0" });

    // Xử lý khi nhiều action trùng route + method
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    // Chỉ định rule đưa API nào vào document nào
    options.DocInclusionPredicate((version, apiDesc) =>
    {
        if (!apiDesc.TryGetMethodInfo(out MethodInfo method))
            return false;

        // Lấy version từ attribute [ApiVersion] ở method
        var methodVersions = method.GetCustomAttributes(true)
            .OfType<ApiVersionAttribute>()
            .SelectMany(attr => attr.Versions);

        // Lấy version từ attribute [ApiVersion] ở controller
        var controllerVersions = method.DeclaringType?
            .GetCustomAttributes(true)
            .OfType<ApiVersionAttribute>()
            .SelectMany(attr => attr.Versions);

        // Gộp lại toàn bộ version
        var allVersions = methodVersions.Union(controllerVersions).Distinct();

        // Kiểm tra xem version hiện tại có thuộc document không
        return allVersions.Any(v => v.ToString() == version);
    });
});


// Add API versioning
builder.Services.AddApiVersioning(options =>
{
    // Phiên bản mặc định
    options.DefaultApiVersion = new ApiVersion(1, 0);
    
    // Nếu client không chỉ định version, dùng mặc định
    options.AssumeDefaultVersionWhenUnspecified = true;
    
    // Bật thông tin version trong response headers
    options.ReportApiVersions = true;
    
    // Đọc version từ query string ?version=...
    options.ApiVersionReader = new QueryStringApiVersionReader("version");
});

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
