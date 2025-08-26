using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValidateNestedComplex.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}