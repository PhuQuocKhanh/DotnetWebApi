using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoMapperConditional.DTOs
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<OrderDTO> Orders { get; set; } = new();
    }
}