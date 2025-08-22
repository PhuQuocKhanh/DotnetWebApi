using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAPIValidation.DTOs;
using FluentValidation;

namespace FluentAPIValidation.Validators
{
    public class ProductBaseUpdateValidator : AbstractValidator<ProductBaseUpdateDTO>
    {
         public ProductBaseUpdateValidator()
        {
            Include(new ProductBaseDTOValidator<ProductBaseUpdateDTO>());
            RuleFor(p => p.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be greater than 0.");
        }
    }
}