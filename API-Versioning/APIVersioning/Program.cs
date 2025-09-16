using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Cấu hình Swagger cho nhiều version
builder.Services.AddSwaggerGen(options =>
{
    // Swagger doc cho version 1.0
    options.SwaggerDoc("1.0", new OpenApiInfo
    {
        Title = "API Version",
        Version = "1.0"
    });

    // Swagger doc cho version 2.0
    options.SwaggerDoc("2.0", new OpenApiInfo
    {
        Title = "API Version",
        Version = "2.0"
    });

    // Resolve conflict khi trùng route + method
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    // Lọc endpoint theo ApiVersion
    options.DocInclusionPredicate((version, apiDesc) =>
    {
        if (!apiDesc.TryGetMethodInfo(out MethodInfo method))
            return false;

        var methodVersions = method.GetCustomAttributes(true)
            .OfType<ApiVersionAttribute>()
            .SelectMany(attr => attr.Versions);

        var controllerVersions = method.DeclaringType?
            .GetCustomAttributes(true)
            .OfType<ApiVersionAttribute>()
            .SelectMany(attr => attr.Versions) ?? Enumerable.Empty<ApiVersion>();

        var allVersions = methodVersions.Union(controllerVersions).Distinct();

        return allVersions.Any(v => v.ToString() == version);
    });
});
// Thêm API Versioning
// Add API versioning configuration
// builder.Services.AddApiVersioning(options =>
// {
//     // Nếu client không chỉ định version → dùng version mặc định
//     options.AssumeDefaultVersionWhenUnspecified = true;
//     // Set version mặc định (ở đây là 1.0)
//     options.DefaultApiVersion = new ApiVersion(1, 0);
//     // Trả về thông tin version được hỗ trợ trong response header
//     options.ReportApiVersions = true;
//     // Sử dụng URL segment cho versioning
//     // Ví dụ: /api/v1/products
//     options.ApiVersionReader = new UrlSegmentApiVersionReader();
// });

// Add API Versioning Header
// builder.Services.AddApiVersioning(options =>
// {
//     options.AssumeDefaultVersionWhenUnspecified = true; // Dùng default nếu client không truyền version
//     options.DefaultApiVersion = new ApiVersion(1, 0);   // Default = v1.0
//     options.ReportApiVersions = true;                   // Trả về thông tin version hỗ trợ trong response header
//     options.ApiVersionReader = new HeaderApiVersionReader("api-version"); // Đọc version từ header
// });

// Add API versioning configuration Media Type
builder.Services.AddApiVersioning(options =>
{
    // Cho phép dùng default version nếu client không chỉ định
    options.AssumeDefaultVersionWhenUnspecified = true;
    // Đặt default version là 1.0
    options.DefaultApiVersion = new ApiVersion(1, 0);
    // Thêm header vào response để báo client biết các version được hỗ trợ
    options.ReportApiVersions = true;
    // Kích hoạt Media Type Versioning
    options.ApiVersionReader = new MediaTypeApiVersionReader("v");
    // "v" là tên tham số trong Accept header, ví dụ: application/json;v=2.0
});


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/1.0/swagger.json", "API Version 1.0");
                    options.SwaggerEndpoint("/swagger/2.0/swagger.json", "API Version 2.0");
                });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
