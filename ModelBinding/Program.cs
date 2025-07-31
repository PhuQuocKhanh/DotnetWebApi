using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelBinding.Data;
using ModelBinding.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddControllers(options =>
// {
//     // Loại bỏ JSON formatter mặc định
//     options.OutputFormatters.RemoveType<Microsoft.AspNetCore.Mvc.Formatters.SystemTextJsonOutputFormatter>();
// }).AddXmlSerializerFormatters(); // Thêm XML formatter

// builder.Services.AddControllers(options =>
// {
//     // Enable 406 Not Acceptable status code
//     options.ReturnHttpNotAcceptable = true;
// })
// // Optionally, configure JSON options or other formatter settings
// .AddJsonOptions(options =>
// {
//     // Configure JSON serializer settings if needed
//     options.JsonSerializerOptions.PropertyNamingPolicy = null;
// });

builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    // Ghi đè phản hồi mặc định khi ModelState không hợp lệ
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        // Lấy danh sách các lỗi từ ModelState
                        var errors = context.ModelState
                                    .Where(e => e.Value?.Errors.Count > 0)
                                    .Select(e => new FieldError
                                    {
                                        Field = e.Key,
                                        // Option 1: Use only the first error message
                                        // Error = e.Value.Errors.FirstOrDefault()?.ErrorMessage
                                        // Join multiple error messages into a single string separated by semicolons
                                        Error = string.Join("; ", e.Value?.Errors.Select(x => x.ErrorMessage ?? string.Empty) ?? Array.Empty<string>())
                                    }).ToList();

                        // Tạo object phản hồi lỗi tùy chỉnh
                        var errorResponse = new ValidationErrorResponse
                        {
                            //The HTTP status code (400 for Bad Request).
                            StatusCode = 400,
                            // A general message indicating that validation failed.
                            Message = "Validation Failed",
                            // An array containing details about each validation error, including the field name and associated error messages.
                            Errors = errors
                        };

                        return new BadRequestObjectResult(errorResponse)
                        {
                            ContentTypes = { "application/json" } // Đảm bảo phản hồi là JSON
                        };
                    };
                })
                .AddXmlSerializerFormatters() // Enable Xml Formatters
                .AddJsonOptions(options =>
                {
                    // This will use the property names as defined in the C# model
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
                
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 

builder.Services.AddDbContext<ECommerceDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("EFCoreDBConnection")));


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
