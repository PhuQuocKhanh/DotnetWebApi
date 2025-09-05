using JWTDemo.Data;
using JWTDemo.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Đăng ký các Controller và cấu hình JSON serialization
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Giữ nguyên tên thuộc tính (không chuyển thành camelCase)
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();   

// Đăng ký EF Core DbContext với SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EFCoreDBConnection")));
// Đăng ký dịch vụ cache trong bộ nhớ
builder.Services.AddMemoryCache();
// Đăng ký ClientCacheService với lifetime là Singleton
builder.Services.AddSingleton<IClientCacheService, ClientCacheService>();
// Đăng ký các service ứng dụng với lifetime là Scoped (tạo mới cho mỗi request)
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();

Lazy<IClientCacheService>? clientCacheInstance = null;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // Thiết lập các tham số xác thực token
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, // Xác thực bên phát hành
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidateAudience = false, // Audience sẽ được xác thực thủ công sau
                        ValidateIssuerSigningKey = true, // Xác thực khóa ký
                        ValidateLifetime = true, // Xác thực token còn hạn sử dụng

                        // Lấy khóa ký một cách linh động dựa trên claim "client_id",
                        // bằng cách tìm client tương ứng và lấy secret key từ cache.
                        IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
                        {
                            var jwtToken = new JwtSecurityToken(token);
                            // Trích xuất claim "client_id" để biết client nào đã ký token này
                            var clientId = jwtToken.Claims.FirstOrDefault(c => c.Type == "client_id")?.Value;

                            if (string.IsNullOrEmpty(clientId) || clientCacheInstance == null)
                                return Enumerable.Empty<SecurityKey>();

                            // Lấy thông tin client từ cache một cách đồng bộ
                            var client = clientCacheInstance.Value.GetClientByClientIdAsync(clientId).Result;

                            if (client == null)
                                return Enumerable.Empty<SecurityKey>();

                            // Chuyển đổi client secret (Base64) thành byte array
                            var keyBytes = Convert.FromBase64String(client.ClientSecret);
                            // Tạo khóa đối xứng để xác thực chữ ký
                            return new[] { new SymmetricSecurityKey(keyBytes) };
                        }
                    };
                    // Các sự kiện để kiểm tra thêm sau khi token được xác thực
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            // Trích xuất claim "client_id" từ token đã được xác thực
                            var clientId = context.Principal?.FindFirst("client_id")?.Value;
                            if (string.IsNullOrEmpty(clientId))
                            {
                                context.Fail("ClientId claim missing."); // Lỗi nếu thiếu claim
                                return;
                            }
                            if (clientCacheInstance == null)
                            {
                                context.Fail("Client Cache Instance is null");
                                return;
                            }
                            // Lấy thông tin client từ cache
                            var client = await clientCacheInstance.Value.GetClientByClientIdAsync(clientId);
                            if (client == null)
                            {
                                context.Fail("Invalid client."); // Lỗi nếu không tìm thấy client
                                return;
                            }
                            // Trích xuất claim audience và so sánh với URL của client
                            var audClaim = context.Principal?.FindFirst(JwtRegisteredClaimNames.Aud)?.Value;
                            if (audClaim != client.ClientURL)
                            {
                                context.Fail("Invalid audience."); // Lỗi nếu audience không khớp
                                return;
                            }
                        }
                    };
                });

var app = builder.Build();

  // Khởi tạo instance của client cache sau khi DI container đã được build
clientCacheInstance = new Lazy<IClientCacheService>(() =>
    app.Services.GetRequiredService<IClientCacheService>());

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
