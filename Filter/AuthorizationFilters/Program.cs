using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Read the JWT secret key from the appsettings.json configuration file.
// This key will be used to sign and validate JWT tokens.
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "b35a8a921d5c93ed8c011a068936ffe7";
// Register MVC controllers with the application.
// Also, configure JSON options to keep property names as defined in the C# models.
builder.Services.AddControllers(options =>
{
    // Apply AuthorizeFilter globally
    options.Filters.Add(new AuthorizeFilter());
})
.AddJsonOptions(options =>
{
    // Disable camelCase in JSON output, preserve property names as defined in C# classes
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// Đăng ký dịch vụ Xác thực với scheme là JWT Bearer
builder.Services.AddAuthentication(options =>
{
    // Hai tùy chọn này đặt JWT Bearer làm scheme mặc định cho việc xác thực và thách thức.
    // Điều này có nghĩa là middleware sẽ tìm kiếm JWT token trong các request đến theo mặc định.
    // Đặt scheme mặc định được sử dụng để xác thực — cách ứng dụng sẽ cố gắng xác thực các request đến
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // Đặt scheme thách thức mặc định — cách ứng dụng sẽ thách thức các request không được ủy quyền
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Cấu hình các tham số để xác thực các JWT token đến
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // KHÔNG xác thực nhà phát hành (claim "iss" của token)
        ValidateIssuer = false,
        // KHÔNG xác thực đối tượng (claim "aud" của token)
        ValidateAudience = false,
        // Đảm bảo chữ ký của token khớp với khóa ký (để xác minh tính toàn vẹn của token)
        ValidateIssuerSigningKey = true,
        // Khóa được sử dụng để ký token — phải khớp với khóa được sử dụng để tạo token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) // Sử dụng khóa đối xứng từ cấu hình để xác thực token.
    };
});

// Định nghĩa một chính sách (policy)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminAndManager", policy =>
        policy.RequireRole("Admin")    // phải có vai trò Admin
                .RequireRole("Manager")  // VÀ cũng phải có vai trò Manager
    );
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthorizationFilterDemo API", Version = "v1" });

                // Thêm định nghĩa bảo mật cho JWT Bearer Token
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                // Thêm yêu cầu bảo mật để áp dụng token cho các endpoint có attribute [Authorize]
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

 // Thêm middleware xác thực để xác thực JWT token trong các request đến.
app.UseAuthentication();  // PHẢI đặt trước UseAuthorization

// Thêm middleware phân quyền để kiểm tra quyền của người dùng khi truy cập tài nguyên.
app.UseAuthorization();

app.MapControllers();

app.Run();
