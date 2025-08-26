using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using ValidateNestedComplex.DTOs;

namespace ValidateNestedComplex.Validator
{
    public class OrderItemDTOValidator : AbstractValidator<OrderItemDTO>
    {
        public OrderItemDTOValidator()
        {
            RuleFor(oi => oi.ProductId)
                .GreaterThan(0).WithMessage("ProductId phải lớn hơn 0.");
            RuleFor(oi => oi.Quantity)
                .GreaterThan(0).WithMessage("Số lượng phải lớn hơn 0.");
        }
    }
}