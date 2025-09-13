using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.DTOs
{
    public class ValidateSSOTokenResponseDTO
    {
        public string Token { get; set; } = null!;
        public UserDetails? UserDetails { get; set; }
    }
    public class UserDetails
    {
        public string UserId { get; set; } = null!;
        public string? Username { get; set; }
        public string? Email { get; set; }
    }
}