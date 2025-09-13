using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientApplicationOne.Models
{
    public class ValidateSSOResponseModel
    {
        public string? Token { get; set; }
        public UserDetailsModel? UserDetails { get; set; }
    }
}