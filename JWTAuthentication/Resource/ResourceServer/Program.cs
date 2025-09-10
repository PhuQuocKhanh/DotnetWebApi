using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Sử dụng tên thuộc tính như đã định nghĩa trong model C#
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidateAudience = false, // Không xác thực đối tượng nhận token (Audience)
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        // Hàm để lấy khóa ký từ endpoint JWKS
                        IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
                        {
                            var httpClient = new HttpClient();
                            var jwks = httpClient.GetStringAsync(builder.Configuration["Jwt:JWKS"]).Result;
                            var keys = new JsonWebKeySet(jwks).Keys;
                            return keys;
                        }
                    };
                });

 builder.Services.AddAuthorization(options =>
                                    {
                                        // Kịch bản 1: Chỉ xác thực (không có role cụ thể)
                                        // Chính sách này chỉ yêu cầu người dùng phải được xác thực
                                        options.AddPolicy("AuthenticatedUser", policy =>
                                        {
                                            policy.RequireAuthenticatedUser();
                                        });

                                        // Kịch bản 2: Yêu cầu role Admin
                                        options.AddPolicy("AdminOnly", policy =>
                                        {
                                            policy.RequireRole("Admin");
                                        });

                                        // Kịch bản 3: Yêu cầu role User
                                        options.AddPolicy("UserOnly", policy =>
                                        {
                                            policy.RequireRole("User");
                                        });

                                        // Kịch bản 4: Yêu cầu một trong hai role Admin HOẶC User
                                        // Với RequireRole, liệt kê nhiều role là điều kiện OR
                                        options.AddPolicy("AdminOrUser", policy =>
                                        {
                                            policy.RequireRole("Admin", "User");
                                        });

                                        // Kịch bản 5: Phân quyền với cả hai role Admin và User
                                        // Nhiều lời gọi RequireRole trong một chính sách được xử lý như điều kiện AND
                                        options.AddPolicy("AdminAndUser", policy =>
                                            policy.RequireRole("Admin")
                                                .RequireRole("User"));
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

app.UseAuthentication(); // Bật middleware xác thực
app.UseAuthorization(); // Bật middleware ủy quyền
app.MapControllers();

app.Run();
