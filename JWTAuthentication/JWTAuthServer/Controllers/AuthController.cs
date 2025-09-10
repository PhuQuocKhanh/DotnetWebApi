using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JWTAuthServer.Data;
using JWTAuthServer.DTOs;
using JWTAuthServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // Các trường private để giữ cấu hình và database context
        // Giữ các thiết lập cấu hình từ appsettings.json hoặc biến môi trường
        private readonly IConfiguration _configuration;
        // Database context để tương tác với cơ sở dữ liệu
        private readonly ApplicationDbContext _context;

        // Constructor thực hiện dependency injection cho IConfiguration và ApplicationDbContext
        public AuthController(IConfiguration configuration, ApplicationDbContext context)
        {
            // Gán IConfiguration được inject vào trường private
            _configuration = configuration;
            // Gán ApplicationDbContext được inject vào trường private
            _context = context;
        }

        // Định nghĩa endpoint Login, phản hồi các yêu cầu POST tại 'api/Auth/Login'
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            // Xác thực model đầu vào dựa trên các data annotation trong LoginDTO
            if (!ModelState.IsValid)
            {
                // Nếu model không hợp lệ, trả về 400 Bad Request cùng với các lỗi xác thực
                return BadRequest(ModelState);
            }

            // Truy vấn bảng Clients để xác minh ClientId được cung cấp có tồn tại hay không
            var client = _context.Clients.FirstOrDefault(c => c.ClientId == loginDto.ClientId);

            // Nếu client không tồn tại, trả về phản hồi 401 Unauthorized
            if (client == null)
            {
                return Unauthorized("Invalid client credentials.");
            }

            // Lấy người dùng từ bảng Users bằng cách so khớp email (không phân biệt chữ hoa/thường)
            // Bao gồm cả UserRoles và Role liên quan để sử dụng sau này
            var user = await _context.Users
                .Include(u => u.UserRoles)       // Bao gồm thuộc tính điều hướng UserRoles
                .ThenInclude(ur => ur.Role)     // Sau đó bao gồm Role trong mỗi UserRole
                .FirstOrDefaultAsync(u => u.Email.ToLower() == loginDto.Email.ToLower());

            // Nếu người dùng không tồn tại, trả về phản hồi 401 Unauthorized
            if (user == null)
            {
                // Vì lý do bảo mật, tránh chỉ rõ client hay user không hợp lệ
                return Unauthorized("Invalid credentials.");
            }

            // Xác minh mật khẩu được cung cấp với mật khẩu đã được băm trong CSDL bằng BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);

            // Nếu mật khẩu không hợp lệ, trả về phản hồi 401 Unauthorized
            if (!isPasswordValid)
            {
                // Một lần nữa, tránh chỉ rõ client hay user không hợp lệ
                return Unauthorized("Invalid credentials.");
            }

            // Tại thời điểm này, xác thực thành công. Tiến hành tạo JWT token.
            var token = GenerateJwtToken(user, client);

            // Tạo Refresh Token
            var refreshToken = GenerateRefreshToken();

            // Băm refresh token trước khi lưu trữ
            var hashedRefreshToken = HashToken(refreshToken);

            // Tạo entity RefreshToken
            var refreshTokenEntity = new RefreshToken
            {
                Token = hashedRefreshToken,
                UserId = user.Id,
                ClientId = client.Id,
                // Refresh token được đặt hết hạn sau 7 ngày (bạn có thể điều chỉnh)
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };

            // Refresh token đã được băm, cùng với thông tin user và client liên quan, được lưu vào bảng RefreshTokens
            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            // Trả về cả hai token cho client
            return Ok(new TokenResponseDTO
            {
                Token = token,
                RefreshToken = refreshToken
            });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDTO requestDto)
        {
            // Đảm bảo request đầu vào chứa cả RefreshToken và ClientId.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Trích xuất user ID từ claims của access token để đảm bảo refresh token bị thu hồi thuộc về người dùng đã xác thực.
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Invalid access token.");
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Invalid user ID in access token.");
            }

            // Hash refresh token đầu vào để so sánh với hash đã lưu
            var hashedToken = HashToken(requestDto.RefreshToken);

            // Sử dụng hashed token, ClientId và User Id để tìm thực thể RefreshToken tương ứng trong database.
            var storedRefreshToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .Include(rt => rt.Client)
                .FirstOrDefaultAsync(rt => rt.Token == hashedToken && rt.Client.ClientId == requestDto.ClientId && rt.UserId == userId);

            // Kiểm tra xem refresh token có tồn tại không.
            if (storedRefreshToken == null)
            {
                return Unauthorized("Invalid refresh token.");
            }

            // Đảm bảo token chưa bị thu hồi để tránh các thao tác dư thừa.
            if (storedRefreshToken.IsRevoked)
            {
                return BadRequest("Refresh token is already revoked.");
            }

            // Thu hồi refresh token hiện tại
            // Đặt IsRevoked thành true và cập nhật timestamp RevokedAt.
            storedRefreshToken.IsRevoked = true;
            storedRefreshToken.RevokedAt = DateTime.UtcNow;

            // Nếu IsLogoutFromAllDevices là true, thu hồi tất cả refresh token của người dùng.
            if (requestDto.IsLogoutFromAllDevices)
            {
                var userRefreshTokens = await _context.RefreshTokens
                    .Where(rt => rt.UserId == storedRefreshToken.UserId && !rt.IsRevoked)
                    .ToListAsync();

                foreach (var token in userRefreshTokens)
                {
                    token.IsRevoked = true;
                    token.RevokedAt = DateTime.UtcNow;
                }
            }

            // Lưu các thay đổi vào database.
            await _context.SaveChangesAsync();

            // Trả về thông báo thành công khi thu hồi token thành công.
            return Ok(new
            {
                Message = "Logout successful. Refresh token has been revoked."
            });
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Băm refresh token nhận được để so sánh với chuỗi băm đã lưu
            var hashedToken = HashToken(requestDto.RefreshToken);

            // Kiểm tra xem refresh token có tồn tại và khớp với ClientId được cung cấp hay không.
            // Lấy refresh token từ cơ sở dữ liệu
            var storedRefreshToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .ThenInclude(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Include(rt => rt.Client)
                .FirstOrDefaultAsync(rt => rt.Token == hashedToken && rt.Client.ClientId == requestDto.ClientId);

            if (storedRefreshToken == null)
            {
                return Unauthorized("Invalid refresh token.");
            }

            // Đảm bảo token chưa bị thu hồi.
            if (storedRefreshToken.IsRevoked)
            {
                return Unauthorized("Refresh token has been revoked.");
            }

            // Đảm bảo token chưa hết hạn.
            if (storedRefreshToken.ExpiresAt < DateTime.UtcNow)
            {
                return Unauthorized("Refresh token has expired.");
            }

            // Lấy thông tin user và client
            var user = storedRefreshToken.User;
            var client = storedRefreshToken.Client;

            // Refresh token hiện tại được đánh dấu là đã thu hồi để ngăn việc tái sử dụng.
            storedRefreshToken.IsRevoked = true;
            storedRefreshToken.RevokedAt = DateTime.UtcNow;

            // Tạo một refresh token mới
            var newRefreshToken = GenerateRefreshToken();
            var hashedNewRefreshToken = HashToken(newRefreshToken);
            var newRefreshTokenEntity = new RefreshToken
            {
                Token = hashedNewRefreshToken,
                UserId = user.Id,
                ClientId = client.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7), // Điều chỉnh nếu cần
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };

            // Lưu trữ refresh token mới
            _context.RefreshTokens.Add(newRefreshTokenEntity);

            // Tạo JWT access token mới
            var newJwtToken = GenerateJwtToken(user, client);

            // Lưu các thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Trả về các token mới cho client
            return Ok(new TokenResponseDTO
            {
                Token = newJwtToken,
                RefreshToken = newRefreshToken
            });
        }


        // Phương thức private chịu trách nhiệm tạo JWT token cho người dùng đã xác thực
        private string GenerateJwtToken(User user, Client client)
        {
            // Lấy khóa ký đang hoạt động từ bảng SigningKeys
            var signingKey = _context.SigningKeys.FirstOrDefault(k => k.IsActive);

            // Nếu không tìm thấy khóa ký nào đang hoạt động, ném ra một exception
            if (signingKey == null)
            {
                throw new Exception("No active signing key available.");
            }

            // Chuyển đổi chuỗi khóa riêng (private key) đã được mã hóa Base64 trở lại thành mảng byte
            var privateKeyBytes = Convert.FromBase64String(signingKey.PrivateKey);

            // Tạo một instance RSA mới cho các hoạt động mã hóa
            var rsa = RSA.Create();

            // Nhập khóa riêng RSA vào instance RSA
            rsa.ImportRSAPrivateKey(privateKeyBytes, out _);

            // Tạo một RsaSecurityKey mới sử dụng instance RSA
            var rsaSecurityKey = new RsaSecurityKey(rsa)
            {
                // Gán Key ID để liên kết JWT với khóa công khai (public key) chính xác
                KeyId = signingKey.KeyId
            };

            // Định nghĩa thông tin xác thực ký (signing credentials) bằng cách sử dụng khóa bảo mật RSA và chỉ định thuật toán
            var creds = new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256);

            // Khởi tạo một danh sách các claim để đưa vào JWT
            var claims = new List<Claim>
            {
                // Claim Subject (sub) với ID của người dùng
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                // Claim JWT ID (jti) với một định danh duy nhất cho token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                // Claim Name với tên của người dùng
                new Claim(ClaimTypes.Name, user.Firstname),
                // Claim NameIdentifier với email của người dùng
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                // Claim Email với email của người dùng
                new Claim(ClaimTypes.Email, user.Email)
            };

            // Lặp qua các vai trò của người dùng và thêm mỗi vai trò như một Role claim
            foreach (var userRole in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
            }

            // Định nghĩa các thuộc tính của JWT token, bao gồm issuer, audience, claims, thời gian hết hạn, và signing credentials
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],       // Bên phát hành token, thường là URL của ứng dụng của bạn
                audience: client.ClientURL,                 // Đối tượng dự kiến nhận token, thường là URL của client
                claims: claims,                             // Danh sách các claim để đưa vào token
                expires: DateTime.UtcNow.AddHours(1),       // Thời gian hết hạn của token được đặt là 1 giờ kể từ bây giờ
                signingCredentials: creds                   // Thông tin xác thực được sử dụng để ký token
            );

            // Tạo một trình xử lý JWT token để serialize token
            var tokenHandler = new JwtSecurityTokenHandler();

            // Serialize token thành một chuỗi
            var token = tokenHandler.WriteToken(tokenDescriptor);

            // Trả về chuỗi JWT token đã được serialize
            return token;
        }

        // Phương thức trợ giúp để tạo một refresh token ngẫu nhiên và an toàn
        private string GenerateRefreshToken()
        {
            // Một chuỗi ngẫu nhiên an toàn được tạo bằng RandomNumberGenerator
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        // Phương thức trợ giúp để băm token trước khi lưu trữ
        private string HashToken(string token)
        {
            // Refresh token được băm bằng SHA256 trước khi lưu vào CSDL để ngăn chặn việc trộm cắp token làm ảnh hưởng đến bảo mật.
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(hashedBytes);
        }
        
    }
}