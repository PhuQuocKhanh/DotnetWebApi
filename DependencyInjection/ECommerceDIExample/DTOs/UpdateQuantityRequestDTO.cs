using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceDIExample.DTOs
{
    public class UpdateQuantityRequestDTO
    {
        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "CartItemId is required.")]
        public int CartItemId { get; set; }
        [Required(ErrorMessage = "NewQuantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "NewQuantity must be at least 1.")]
        public int NewQuantity { get; set; }
    }
}