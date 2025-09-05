using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientSecretKeyGenerator.DTOs
{
    public class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ClientId { get; set; } = null!;
    }
}