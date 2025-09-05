using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BasicAuthenticationDemo.Models
{
   // Trình xử lý xác thực tùy chỉnh xử lý Basic Authentication.
    // Kế thừa từ AuthenticationHandler<TOptions> với TOptions là AuthenticationSchemeOptions.
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        // DbContext để truy vấn thông tin người dùng trong quá trình xác thực.
        private readonly ApplicationDbContext _context;

        // Constructor để inject các dependency cần thiết và thêm DbContext.
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ApplicationDbContext context)
            : base(options, logger, encoder)
        {
            _context = context;
        }

        // Phương thức cốt lõi thực hiện logic xác thực trên mỗi yêu cầu.
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                // Bước 1: Kiểm tra xem header "Authorization" có tồn tại không.
                if (!Request.Headers.ContainsKey("Authorization"))
                {
                    return AuthenticateResult.Fail("Missing Authorization Header");
                }

                // Bước 2: Lấy giá trị của header "Authorization".
                var authorizationHeader = Request.Headers["Authorization"].ToString();

                // Bước 3: Phân tích header để tách scheme và parameter.
                if (!AuthenticationHeaderValue.TryParse(authorizationHeader, out var headerValue))
                {
                    return AuthenticateResult.Fail("Invalid Authorization Header");
                }

                // Bước 4: Xác minh scheme là "Basic" (không phân biệt chữ hoa/thường).
                if (!"Basic".Equals(headerValue.Scheme, StringComparison.OrdinalIgnoreCase))
                {
                    return AuthenticateResult.Fail("Invalid Authorization Scheme");
                }

                // Bước 5: Giải mã chuỗi Base64 ("username:password").
                var credentialBytes = Convert.FromBase64String(headerValue.Parameter!);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);

                // Bước 6: Xác thực chuỗi giải mã chứa đúng username và password.
                if (credentials.Length != 2)
                {
                    return AuthenticateResult.Fail("Invalid Authorization Header");
                }

                // Bước 7: Trích xuất username (ở đây là Email) và password.
                var email = credentials[0];
                var password = credentials[1];

                // Bước 8: Truy vấn cơ sở dữ liệu để tìm người dùng theo email.
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
                if (user == null || !PasswordHasher.VerifyPassword(user.PasswordHash, password))
                {
                    return AuthenticateResult.Fail("Invalid Username or Password");
                }

                // Bước 9: Tạo các claim đại diện cho danh tính và vai trò của người dùng.
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Email)
                };

                // Bước 10: Tách các vai trò bằng dấu phẩy và thêm nhiều claim.
                var roles = user.Role.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                // Bước 11: Tạo một ClaimsIdentity với tên của scheme xác thực.
                var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);

                // Bước 12: Tạo ClaimsPrincipal chứa ClaimsIdentity.
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Bước 13: Tạo một AuthenticationTicket, gói gọn danh tính người dùng (ClaimsPrincipal).
                var authenticationTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);

                // Bước 14: Trả về kết quả thành công với AuthenticationTicket.
                return AuthenticateResult.Success(authenticationTicket);
            }
            catch
            {
                return AuthenticateResult.Fail("Error occurred during authentication");
            }
        }
    }
}