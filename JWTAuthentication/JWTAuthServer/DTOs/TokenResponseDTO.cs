using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTAuthServer.DTOs
{
    public class TokenResponseDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}