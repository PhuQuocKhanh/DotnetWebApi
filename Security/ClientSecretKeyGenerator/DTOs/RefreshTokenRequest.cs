using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientSecretKeyGenerator.DTOs
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = null!;
        public string ClientId { get; set; } = null!;
    }
}