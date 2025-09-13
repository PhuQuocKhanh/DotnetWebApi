using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.DTOs
{
    public class ValidateSSOTokenRequestDTO
    {
        [Required(ErrorMessage = "SSOToken is Required")]
        public string SSOToken { get; set; } = null!;
    }
}