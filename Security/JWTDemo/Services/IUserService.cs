using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTDemo.Data;
using JWTDemo.DTOs;
using JWTDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTDemo.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(UserRegisterDTO registerDto);
        Task<AuthResponseDTO?> AuthenticateUserAsync(UserLoginDTO loginDto, string ipAddress);
        Task<AuthResponseDTO?> RefreshTokenAsync(string refreshToken, string clientId, string ipAddress);
        Task<bool> RevokeRefreshTokenAsync(string refreshToken, string ipAddress);
    }
    
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext; // DbContext để thao tác với database
        private readonly ITokenService _tokenService; // Service để tạo và xác thực token
        private readonly IConfiguration _configuration; // Để truy cập file cấu hình
        private readonly IClientCacheService _clientCacheService; // Service cache để lấy thông tin Client nhanh chóng

        // Constructor inject các dependency
        public UserService(ApplicationDbContext dbContext, ITokenService tokenService, IConfiguration configuration, IClientCacheService clientCacheService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
            _configuration = configuration;
            _clientCacheService = clientCacheService;
        }

        // Đăng ký người dùng mới
        public async Task<bool> RegisterUserAsync(UserRegisterDTO registerDto)
        {
            // Kiểm tra email đã tồn tại chưa
            if (await _dbContext.Users.AnyAsync(u => u.Email == registerDto.Email))
                return false; // Đăng ký thất bại nếu email đã tồn tại

            // Tạo người dùng mới, băm mật khẩu bằng BCrypt để bảo mật
            var user = new User
            {
                Firstname = registerDto.Firstname,
                Lastname = registerDto.Lastname,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                IsActive = true
            };

            // Gán role mặc định "User" cho người dùng mới
            var userRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "User");
            if (userRole != null)
                user.UserRoles.Add(new UserRole { RoleId = userRole.Id, User = user });

            // Thêm người dùng mới vào DbContext và lưu vào database
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return true; // Đăng ký thành công
        }

        // Xác thực đăng nhập và trả về token nếu thành công
        public async Task<AuthResponseDTO?> AuthenticateUserAsync(UserLoginDTO loginDto, string ipAddress)
        {
            // Lấy thông tin người dùng bằng email, kèm theo role; chỉ cho phép người dùng đang hoạt động
            var user = await _dbContext.Users
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.IsActive);

            // Xác thực người dùng tồn tại và mật khẩu khớp
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return null; // Sai thông tin đăng nhập

            // Lấy danh sách tên các role để đưa vào claim của JWT
            var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();

            // Lấy thông tin client bằng ClientId từ cache
            var client = await _clientCacheService.GetClientByClientIdAsync(loginDto.ClientId);
            if (client == null)
            {
                // Thất bại nếu client không tồn tại hoặc không hoạt động
                return null;
            }

            // Tạo JWT access token
            var accessToken = _tokenService.GenerateAccessToken(user, roles, out string jwtId, client);

            // Tạo refresh token liên kết với JWT ID, client, user và IP
            var refreshToken = _tokenService.GenerateRefreshToken(ipAddress, jwtId, client, user.Id);

            // Lưu refresh token vào database
            _dbContext.RefreshTokens.Add(refreshToken);
            await _dbContext.SaveChangesAsync();

            // Đọc thời gian hết hạn của access token từ config
            var accessTokenExpiryMinutes = int.TryParse(_configuration["JwtSettings:AccessTokenExpirationMinutes"], out var val) ? val : 15;

            // Trả về DTO chứa các token và thông tin hết hạn
            return new AuthResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(accessTokenExpiryMinutes)
            };
        }

        // Làm mới một access token đã hết hạn bằng refresh token và client ID hợp lệ
        public async Task<AuthResponseDTO?> RefreshTokenAsync(string refreshToken, string clientId, string ipAddress)
        {
            // Lấy thông tin client để xác thực
            var client = await _clientCacheService.GetClientByClientIdAsync(clientId);
            if (client == null)
            {
                return null; // Client không hợp lệ
            }

            // Tìm refresh token trong database, kèm theo thông tin user và role
            var existingToken = await _dbContext.RefreshTokens
                .Include(rt => rt.User).ThenInclude(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.ClientId == client.Id);

            // Kiểm tra refresh token: tồn tại, chưa bị thu hồi, chưa hết hạn
            if (existingToken == null || existingToken.IsRevoked || existingToken.Expires <= DateTime.UtcNow)
                return null; // Refresh token không hợp lệ

            // Thu hồi refresh token cũ ngay lập tức để chống tái sử dụng (token rotation)
            existingToken.IsRevoked = true;
            existingToken.RevokedAt = DateTime.UtcNow;

            var user = existingToken.User;
            var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();

            // Tạo access token mới với JWT ID mới
            var accessToken = _tokenService.GenerateAccessToken(user, roles, out string newJwtId, client);
            // Tạo refresh token mới liên kết với JWT ID mới
            var newRefreshToken = _tokenService.GenerateRefreshToken(ipAddress, newJwtId, client, user.Id);

            // Lưu refresh token mới vào database
            _dbContext.RefreshTokens.Add(newRefreshToken);
            await _dbContext.SaveChangesAsync();

            // Đọc thời gian hết hạn access token từ config
            var accessTokenExpiryMinutes = int.TryParse(_configuration["JwtSettings:AccessTokenExpirationMinutes"], out var val) ? val : 15;

            // Trả về cặp token mới
            return new AuthResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.Token,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(accessTokenExpiryMinutes)
            };
        }

        // Thu hồi một refresh token để ngăn việc sử dụng nó trong tương lai (dùng cho chức năng logout)
        public async Task<bool> RevokeRefreshTokenAsync(string refreshToken, string ipAddress)
        {
            // Tìm refresh token trong database
            var existingToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            // Trả về false nếu không tìm thấy hoặc đã bị thu hồi
            if (existingToken == null || existingToken.IsRevoked)
                return false;

            // Đánh dấu token đã bị thu hồi và ghi lại thời gian
            existingToken.IsRevoked = true;
            existingToken.RevokedAt = DateTime.UtcNow;

            // Lưu thay đổi vào database
            await _dbContext.SaveChangesAsync();
            return true; // Thu hồi thành công
        }
    }
}