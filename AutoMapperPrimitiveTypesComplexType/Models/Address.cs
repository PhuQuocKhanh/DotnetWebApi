using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoMapperPrimitiveTypesComplexType.Models
{
    public class Address
    {
        public int AddressId { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        // Khóa ngoại liên kết với User
        public int UserId { get; set; }

        public User User { get; set; }
    }
}