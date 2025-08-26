using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthorizationFilters.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationFilters.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        // Danh sách người dùng được hardcode trong bộ nhớ để demo (mô phỏng một cơ sở dữ liệu người dùng)
        private readonly List<User> _users = new List<User>()
        {
            new User {Id = 1, Email ="Alice@Example.com", Name = "Alice", Password = "alice123", Roles = "Admin,Manager" },
            new User {Id = 2, Email ="Bob@Example.com", Name = "Bob", Password = "bob123", Roles = "User" },
            new User {Id = 3, Email ="Charlie@Example.com", Name = "Charlie", Password = "charlie123", Roles = "Manager,User" }
        };

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Endpoint này có thể được truy cập bởi bất kỳ ai, ngay cả những người dùng chưa được xác thực
        [HttpPost("login")]
        [AllowAnonymous] 
        public IActionResult Login([FromBody] LoginDTO login)
        {
            // Tìm một người dùng trong danh sách hardcode của chúng ta khớp với email và mật khẩu được cung cấp (kiểm tra email không phân biệt chữ hoa chữ thường)
            var user = _users.FirstOrDefault(u =>
                u.Email.Equals(login.Email, StringComparison.OrdinalIgnoreCase)
                && u.Password == login.Password);

            if (user == null)
            {
                // Nếu không có người dùng phù hợp, thông tin xác thực không hợp lệ — trả về mã trạng thái 401 Unauthorized
                return Unauthorized("Invalid username or password");
            }

            // Tạo một danh sách các claim để nhúng vào bên trong JWT token cho người dùng này
            var claims = new List<Claim>
            {
                // Claim để xác định người dùng bằng địa chỉ email của họ
                new Claim(ClaimTypes.Name, user.Email),
                // Claim tùy chỉnh với ID duy nhất của người dùng
                new Claim("UserId", user.Id.ToString()),
            };

            // Thêm các claim cho mỗi vai trò được gán cho người dùng (vai trò là chuỗi được phân tách bằng dấu phẩy)
            var roles = user.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var role in roles)
            {
                // Thêm một claim vai trò cho mỗi vai trò
                claims.Add(new Claim(ClaimTypes.Role, role.Trim()));
            }

            // Tạo khóa bảo mật đối xứng từ khóa bí mật được cấu hình trong appsettings.json
            var secretKey = _configuration.GetValue<string>("JwtSettings:SecretKey") ?? "d3011f8b98bbc1aa1c4ff1a7d4864fc72d9ee150bd682cf4e612d6321f57821d";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            // Chỉ định thông tin xác thực ký bằng thuật toán HMAC SHA256 và khóa đã tạo
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tạo một JWT token nhúng các claim, không có issuer/audience để đơn giản, và thời hạn hết hạn được đặt là 30 phút kể từ bây giờ
            var token = new JwtSecurityToken(
                issuer: null, // Không có nhà phát hành cụ thể.
                audience: null, // Không có đối tượng cụ thể.
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30), // Token có hiệu lực trong 30 phút
                signingCredentials: creds);

            // Chuyển đổi JWT token thành một chuỗi
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Trả về chuỗi JWT token dưới dạng JSON cho client
            return Ok(new { Token = tokenString });
        }
    }
}