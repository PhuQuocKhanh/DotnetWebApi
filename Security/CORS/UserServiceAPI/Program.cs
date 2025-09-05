using Microsoft.EntityFrameworkCore;
using UserServiceAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add controllers and configure JSON options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure EF Core DbContext
builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("EFCoreDBConnection"));
});

builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAllOrigin",
                        builder =>
                        {
                            builder.AllowAnyOrigin()  // Cho phép tất cả Origin
                                .AllowAnyHeader()  // Cho phép tất cả header (như Content-Type)
                                .AllowAnyMethod(); // Cho phép tất cả phương thức HTTP (GET, POST, v.v.)
                        });
                });

// Cấu hình một chính sách CORS cụ thể
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowSpecificOrigin", policy =>
//     {
//         policy
//             // Chỉ cho phép các origin này
//             .WithOrigins("https://localhost:7102", "https://example.com") 
//             // Chỉ cho phép các header này
//             .WithHeaders("Content-Type", "Authorization", "Accept") 
//             // Chỉ cho phép các phương thức HTTP này
//             .WithMethods("GET", "POST", "PUT", "DELETE"); 
//     });
// });

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

// // Áp dụng chính sách CORS đã đặt tên trên toàn cục
// app.UseCors("AllowSpecificOrigin");

app.Run();
