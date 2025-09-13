using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Username is Required")]
        public string Username { get; set; } = null!;
        [EmailAddress(ErrorMessage = "Please provide a valid Email")]
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; } = null!;
    }
}