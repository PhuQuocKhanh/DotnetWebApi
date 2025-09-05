using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace JWTDemo.Models
{
    [Index(nameof(Email), Name = "IX_Unique_Email", IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Tên không được vượt quá 50 ký tự.")]
        public string Firstname { get; set; } = null!;

        [MaxLength(50, ErrorMessage = "Họ không được vượt quá 50 ký tự.")]
        public string? Lastname { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Địa chỉ Email không hợp lệ.")]
        public string Email { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        [Required]
        [MaxLength(100)]
        public string PasswordHash { get; set; } = null!;

        // Thuộc tính điều hướng cho các vai trò.
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        // Thuộc tính điều hướng cho các Refresh Token.
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}