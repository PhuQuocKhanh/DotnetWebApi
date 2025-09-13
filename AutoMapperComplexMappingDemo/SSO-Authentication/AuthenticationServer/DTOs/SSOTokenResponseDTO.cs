using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.DTOs
{
    public class SSOTokenResponseDTO
    {
        public string SSOToken { get; set; } = null!;
    }
}