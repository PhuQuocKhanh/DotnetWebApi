using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAPIValidation.DTOs
{
    public class ProductBaseUpdateDTO : ProductBaseDTO
    {
        public int ProductId { get; set; }
    }
}