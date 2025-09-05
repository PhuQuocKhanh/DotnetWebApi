using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JWTDemo.DTOs
{
    public class UserLoginDTO
    {
        // Email người dùng nhập vào khi đăng nhập.
        [EmailAddress]
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [MaxLength(100, ErrorMessage = "Email phải nhỏ hơn hoặc bằng 100 ký tự.")]
        public string Email { get; set; } = null!;

        // Mật khẩu người dùng nhập vào khi đăng nhập.
        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải dài ít nhất 6 ký tự.")]
        [MaxLength(100, ErrorMessage = "Mật khẩu phải nhỏ hơn hoặc bằng 100 ký tự.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "ClientId là bắt buộc.")]
        public string ClientId { get; set; } = null!;
    }
}