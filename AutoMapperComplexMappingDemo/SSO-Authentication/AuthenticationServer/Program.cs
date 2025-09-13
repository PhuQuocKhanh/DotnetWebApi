using System.Text;
using AuthenticationServer.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Thêm hỗ trợ cho controllers vào ứng dụng.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Tắt Camel Case
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Cấu hình ApplicationDbContext để sử dụng SQL Server với chuỗi kết nối từ tệp cấu hình.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthenticationServerDBConnection")));

// Thêm các dịch vụ ASP.NET Core Identity.
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders(); // Bật hỗ trợ cho Đặt lại Mật khẩu, Xác nhận Email, Xác minh Điện thoại và 2FA.

 builder.Services.AddAuthentication(options =>
            {
                // Skema Xác thực Mặc định
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // Thêm trình xử lý xác thực JwtBearer để kiểm tra các token JWT đến.
            .AddJwtBearer(options =>
            {
                // Cấu hình các tham số để kiểm tra token JWT.
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, // Đảm bảo nhà phát hành (issuer) của token khớp với nhà phát hành dự kiến.
                    ValidateAudience = false, // Đảm bảo đối tượng (audience) của token khớp với đối tượng dự kiến.
                    ValidateLifetime = true, // Kiểm tra xem token đã hết hạn chưa.
                    ValidateIssuerSigningKey = true, // Đảm bảo token được ký bởi một khóa ký tin cậy.
                    ValidIssuer = builder.Configuration["Jwt:Issuer"], // Nhà phát hành dự kiến, lấy từ cấu hình (appsettings.json).
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty)) // Khóa đối xứng dùng để ký JWT, cũng từ cấu hình (appsettings.json).
                };
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

// Kích hoạt middleware xác thực.
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
