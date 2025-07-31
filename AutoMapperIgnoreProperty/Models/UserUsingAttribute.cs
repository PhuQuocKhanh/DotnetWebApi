using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapperIgnoreProperty.Attribute;

namespace AutoMapperIgnoreProperty.Models
{
    public class UserUsingAttribute
    {
        public int Id { get; set; }
        public string Username { get; set; }

        [NoMap]
        public string Password { get; set; }

        [NoMap]
        public string SecurityToken { get; set; }
    }
}