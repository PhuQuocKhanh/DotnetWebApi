using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordHashingDemo.Models;

namespace PasswordHashingDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _context;

        public UserController(UserDbContext context)
        {
            _context = context;
        }

        // POST: api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDTO model)
        {
            // Xác thực model đầu vào
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra xem email đã tồn tại chưa
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                return BadRequest("Email is already in use.");

            // Tạo password hash và salt
            PasswordHasher.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // Tạo thực thể user
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            // Thêm user vào database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Vì lý do bảo mật, không trả về password hash và salt
            return Ok(new { Message = "User registered successfully." });
        }

        // POST: api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            // Xác thực model đầu vào
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Lấy thông tin người dùng theo email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
                return Unauthorized("Invalid email or password.");

            // Xác minh mật khẩu
            if (!PasswordHasher.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
                return Unauthorized("Invalid email or password.");

            // TODO: Tạo JWT hoặc token khác nếu cần
            return Ok(new { Message = "User logged in successfully." });
        }
    }
}