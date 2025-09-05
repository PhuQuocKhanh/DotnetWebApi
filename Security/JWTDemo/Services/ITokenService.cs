using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JWTDemo.Models;
using Microsoft.IdentityModel.Tokens;

namespace JWTDemo.Services
{
    public interface ITokenService
    {
        // Tạo Access Token (JWT)
        string GenerateAccessToken(User user, IList<string> roles, out string jwtId, Client client);
        // Tạo Refresh Token
        RefreshToken GenerateRefreshToken(string ipAddress, string jwtId, Client client, int userId);
    }

    public class TokenService : ITokenService
    {
        // IConfiguration để truy cập các giá trị trong appsettings.json
        private readonly IConfiguration _configuration;

        // Constructor inject IConfiguration
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Tạo một JWT Access Token cho người dùng đã xác thực.
        public string GenerateAccessToken(User user, IList<string> roles, out string jwtId, Client client)
        {
            // Khởi tạo handler để tạo và serialize token
            var tokenHandler = new JwtSecurityTokenHandler();
            // Giải mã client secret đã được mã hóa Base64 thành mảng byte để ký
            var keyBytes = Convert.FromBase64String(client.ClientSecret);
            var key = new SymmetricSecurityKey(keyBytes);

            // Tạo một định danh duy nhất cho JWT (claim 'jti')
            jwtId = Guid.NewGuid().ToString();

            // Đọc issuer và thời gian hết hạn của token từ file config, có giá trị mặc định
            var issuer = _configuration["JwtSettings:Issuer"] ?? "DefaultIssuer";
            var accessTokenExpirationMinutes = int.TryParse(_configuration["JwtSettings:AccessTokenExpirationMinutes"], out var val) ? val : 15;

            // Định nghĩa các "claims" (thông tin) sẽ được nhúng vào token
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject - định danh người dùng
                new Claim(JwtRegisteredClaimNames.Jti, jwtId), // JWT ID - định danh token, dùng để liên kết với refresh token
                new Claim(JwtRegisteredClaimNames.Email, user.Email), // Email người dùng
                new Claim(JwtRegisteredClaimNames.Iss, issuer), // Issuer - bên phát hành token
                new Claim(JwtRegisteredClaimNames.Aud, client.ClientURL), // Audience - đối tượng sẽ nhận token
                new Claim("client_id", client.ClientId) // Claim tùy chỉnh: client id để biết client nào đã yêu cầu token
            };

            // Thêm các role claim để phân quyền (RBAC)
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Tạo thông tin ký (signing credentials) với khóa đối xứng và thuật toán HMAC SHA256
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            // Định nghĩa bộ mô tả token (token descriptor) chứa claims, thời gian hết hạn, thông tin ký...
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(accessTokenExpirationMinutes),
                SigningCredentials = creds,
                Issuer = issuer,
                Audience = client.ClientURL
            };

            // Tạo token dựa trên descriptor
            var token = tokenHandler.CreateToken(tokenDescriptor);
            // Serialize token thành chuỗi JWT hoàn chỉnh
            return tokenHandler.WriteToken(token);
        }

        // Tạo một Refresh Token được liên kết với một JWT và client.
        public RefreshToken GenerateRefreshToken(string ipAddress, string jwtId, Client client, int userId)
        {
            // Tạo một mảng 64 byte ngẫu nhiên an toàn để làm chuỗi refresh token
            var randomBytes = new byte[64];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            // Đọc thời gian hết hạn của refresh token từ config, mặc định là 7 ngày
            var refreshTokenExpirationDays = int.TryParse(_configuration["JwtSettings:RefreshTokenExpirationDays"], out var val) ? val : 7;

            // Tạo và trả về đối tượng RefreshToken với đầy đủ thuộc tính
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes), // Chuỗi token được mã hóa Base64
                JwtId = jwtId, // Liên kết với JWT ID để có thể thu hồi khi cần
                Expires = DateTime.UtcNow.AddDays(refreshTokenExpirationDays), // Thời gian hết hạn
                CreatedAt = DateTime.UtcNow, // Thời gian tạo
                UserId = userId, // Liên kết với người dùng
                ClientId = client.Id, // Liên kết với client
                IsRevoked = false, // Ban đầu token đang hoạt động (chưa bị thu hồi)
                RevokedAt = null,
                CreatedByIp = ipAddress // Lưu IP đã tạo token (hữu ích cho việc kiểm toán)
            };
        }
    }
}