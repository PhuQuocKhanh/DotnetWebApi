using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceDIExample.DTOs
{
    public class AddItemRequestDTO
    {
        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "ProductId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "ProductId must be a positive integer.")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; } = 1;
    }
}