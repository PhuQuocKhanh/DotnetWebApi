using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthorizationCustomFilter.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationCustomFilter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        [AllowAnonymous] // Cho phép gọi action này mà không cần xác thực
        public IActionResult Login([FromBody] LoginDTO login)
        {
            var user = UserStore.Users.FirstOrDefault(u =>
                    u.Email.Equals(login.Email, StringComparison.OrdinalIgnoreCase)
                    && u.Password == login.Password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            // Tạo các claim để nhúng vào token JWT
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("SubscriptionLevel", user.SubscriptionLevel ?? "Free"),
                new Claim("Department", user.Department ?? "None")
            };

            if (user.SubscriptionExpiresOn != null)
                claims.Add(new Claim("SubscriptionExpiresOn", user.SubscriptionExpiresOn.Value.ToString()));

            var secretKey = _configuration.GetValue<string>("JwtSettings:SecretKey")
                            ?? "d3011f8b98bbc1aa1c4ff1a7d4864fc72d9ee150bd682cf4e612d6321f57821d";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30), // Token có hiệu lực trong 30 phút
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { Token = tokenString });
        }
    }
}