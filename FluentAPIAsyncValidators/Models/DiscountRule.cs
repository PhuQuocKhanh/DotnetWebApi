using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAPIAsyncValidators.Models
{
    public class DiscountRule
    {
         public int DiscountRuleId { get; set; } // Primary Key
        [Column(TypeName = "decimal(10,2)")]
        public decimal MinimumPrice { get; set; } // Minimum price to apply this rule
        [Column(TypeName = "decimal(10,2)")]
        public decimal MaximumDiscount { get; set; } // Maximum discount allowed for products meeting the minimum price
    }
}