using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTDemo.DTOs;
using JWTDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace JWTDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        // Constructor nhận IUserService thông qua Dependency Injection
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        // POST api/auth/register
        // Endpoint để đăng ký người dùng
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO registerDto)
        {
            // Xác thực model đầu vào (ví dụ: các trường bắt buộc, định dạng)
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Trả về lỗi 400 với các lỗi xác thực

            // Gọi UserService để đăng ký người dùng
            var success = await _userService.RegisterUserAsync(registerDto);

            // Nếu đăng ký thất bại (email đã tồn tại), trả về lỗi 400 với thông báo tùy chỉnh
            if (!success)
                return BadRequest(new { message = "Email already exists." });

            // Đăng ký thành công: trả về 200 OK với thông báo thành công
            return Ok(new { message = "User registered successfully." });
        }

        // POST api/auth/login
        // Endpoint để người dùng đăng nhập và tạo token JWT
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO loginDto)
        {
            // Xác thực model đầu vào (email, password, clientId)
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Trả về lỗi 400 với các lỗi xác thực

            // Lấy địa chỉ IP của client để ghi log và tạo refresh token
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            // Gọi UserService để xác thực người dùng và lấy token JWT + refresh token
            var authResponse = await _userService.AuthenticateUserAsync(loginDto, ipAddress);

            // Nếu xác thực thất bại (thông tin không hợp lệ hoặc client không hợp lệ), trả về lỗi 401 Unauthorized
            if (authResponse == null)
                return Unauthorized(new { message = "Invalid credentials or client." });

            // Đăng nhập thành công: trả về 200 OK với các token và thông tin hết hạn
            return Ok(authResponse);
        }

        // POST api/auth/refresh-token
        // Endpoint để lấy một access token mới bằng cách sử dụng refresh token
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDTO refreshRequest)
        {
            // Xác thực model đầu vào (yêu cầu refreshToken và clientId)
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Trả về lỗi 400 với các lỗi xác thực

            // Lấy địa chỉ IP của client (tùy chọn để ghi log/kiểm tra)
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            // Gọi UserService để xác thực refresh token và cấp access & refresh token mới
            var authResponse = await _userService.RefreshTokenAsync(refreshRequest.RefreshToken, refreshRequest.ClientId, ipAddress);

            // Nếu refresh token hoặc client không hợp lệ, trả về lỗi 401 Unauthorized
            if (authResponse == null)
                return Unauthorized(new { message = "Invalid refresh token or client." });

            // Làm mới token thành công: trả về 200 OK với các token mới và thông tin hết hạn
            return Ok(authResponse);
        }
    }
}