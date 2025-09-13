using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthenticationServer.Data;
using AuthenticationServer.DTOs;
using AuthenticationServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        // Constructor inject: UserManager, ApplicationDbContext, IConfiguration
        public AuthenticationController(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
        }

        // POST: api/Authentication/Register
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            var user = new IdentityUser
            {
                UserName = dto.Username,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                return Ok(new { Result = "User Registered Successfully" });
            }

            return BadRequest(result.Errors);
        }

        // POST: api/Authentication/Login
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginDTO dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                var token = GenerateJwtToken(user);
                return Ok(new LoginResponseDTO { Token = token });
            }

            return Unauthorized("Invalid username or password");
        }

        // POST: api/Authentication/GenerateSSOToken
        [HttpPost("GenerateSSOToken")]
        [Authorize]
        public async Task<ActionResult<SSOTokenResponseDTO>> GenerateSSOToken()
        {
            try
            {
                var UserId = User.FindFirstValue("User_Id");
                if (UserId == null) return NotFound("Invalid token");

                var user = await _userManager.FindByIdAsync(UserId);
                if (user == null) return NotFound("User not found");

                var ssoToken = new SSOToken
                {
                    UserId = user.Id,
                    Token = Guid.NewGuid().ToString(),
                    ExpiryDate = DateTime.UtcNow.AddMinutes(10),
                    IsUsed = false
                };

                _context.SSOTokens.Add(ssoToken);
                await _context.SaveChangesAsync();

                return Ok(new SSOTokenResponseDTO { SSOToken = ssoToken.Token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Authentication/ValidateSSOToken
        [HttpPost("ValidateSSOToken")]
        [AllowAnonymous]
        public async Task<ActionResult<ValidateSSOTokenResponseDTO>> ValidateSSOToken([FromBody] ValidateSSOTokenRequestDTO request)
        {
            try
            {
                var ssoToken = await _context.SSOTokens.FirstOrDefaultAsync(t => t.Token == request.SSOToken);

                if (ssoToken == null || ssoToken.IsUsed || ssoToken.IsExpired)
                    return BadRequest("Invalid or expired SSO token");

                ssoToken.IsUsed = true;
                await _context.SaveChangesAsync();

                var user = await _userManager.FindByIdAsync(ssoToken.UserId);
                if (user == null) return BadRequest("Invalid User");

                var newJwtToken = GenerateJwtToken(user);

                return Ok(new ValidateSSOTokenResponseDTO
                {
                    Token = newJwtToken,
                    UserDetails = new UserDetails
                    {
                        UserId = user.Id,
                        Email = user.Email,
                        Username = user.UserName
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Helper: generate JWT token
        private string GenerateJwtToken(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim("User_Id", user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}