using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "d3011f8b98bbc1aa1c4ff1a7d4864fc72d9ee150bd682cf4e612d6321f57821d";

builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Vô hiệu hóa camelCase trong đầu ra JSON, giữ nguyên tên thuộc tính như đã định nghĩa trong class C#
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

// Đăng ký Authentication với scheme là JWT Bearer
builder.Services.AddAuthentication(options =>
            {
                // Đặt scheme mặc định được sử dụng để xác thực
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // Đặt scheme mặc định để thách thức các request không được ủy quyền
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // Cấu hình các tham số để xác thực token JWT đến
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // KHÔNG xác thực nhà phát hành (claim "iss" của token)
                    ValidateIssuer = false,
                    // KHÔNG xác thực đối tượng (claim "aud" của token)
                    ValidateAudience = false,
                    // Đảm bảo chữ ký của token khớp với khóa ký (để xác minh tính toàn vẹn của token)
                    ValidateIssuerSigningKey = true,
                    // Khóa được sử dụng để ký token — phải khớp với khóa đã dùng để tạo token
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
