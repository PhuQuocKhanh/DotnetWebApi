using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceDIExample.DTOs
{
    public class RemoveItemRequestDTO
    {
        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "CartItemId is required.")]
        public int CartItemId { get; set; }
    }
}