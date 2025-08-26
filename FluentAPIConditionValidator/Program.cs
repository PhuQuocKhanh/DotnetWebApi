using FluentAPIConditionValidator.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Giữ nguyên tên thuộc tính gốc khi tuần tự hóa/giải tuần tự hóa.
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
