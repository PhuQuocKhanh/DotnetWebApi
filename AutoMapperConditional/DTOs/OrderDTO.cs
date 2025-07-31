using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoMapperConditional.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsShipped { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal OrderTotal { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }
}