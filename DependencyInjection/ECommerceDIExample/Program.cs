using ECommerceDIExample.Models;
using ECommerceDIExample.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

 // Add services vào container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Cấu hình DbContext
builder.Services.AddDbContext<ECommerceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("EFCoreDBConnection"));
});

// Đăng ký service vào DI container
builder.Services.AddSingleton<ILoggerService, LoggerService>();                  // Singleton
builder.Services.AddSingleton<IGlobalDiscountService, GlobalDiscountService>();  // Singleton
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();         // Scoped
builder.Services.AddTransient<IOrderIdGenerator, OrderIdGenerator>();  
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
