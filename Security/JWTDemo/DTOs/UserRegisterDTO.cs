using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JWTDemo.DTOs
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "Tên là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Tên phải nhỏ hơn hoặc bằng 50 ký tự.")]
        public string Firstname { get; set; } = null!;

        [MaxLength(50, ErrorMessage = "Họ phải nhỏ hơn hoặc bằng 50 ký tự.")]
        public string? Lastname { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Địa chỉ Email không hợp lệ.")]
        public string Email { get; set; } = null!;
    }
}