using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAPIValidation.DTOs;
using FluentValidation;

namespace FluentAPIValidation.Validators
{
    public class ProductBaseCreateDTOValidator : AbstractValidator<ProductBaseCreateDTO>
    {
        public ProductBaseCreateDTOValidator()
        {
            Include(new ProductBaseDTOValidator<ProductBaseCreateDTO>());
            // Rule riêng cho Create (nếu có)
        }
    }
}