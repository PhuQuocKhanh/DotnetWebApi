using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BasicAuthenticationDemo.Models
{
    [Index(nameof(Email), Name = "Index_Email_Unique", IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Full Name must be between 3 and 100 characters.")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email format is invalid.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password hash is required.")]
        public string PasswordHash { get; set; } = null!;

        [Required(ErrorMessage = "Role is required.")]
        [StringLength(50, ErrorMessage = "Role cannot exceed 50 characters.")]
        public string Role { get; set; } = null!;
    }
}