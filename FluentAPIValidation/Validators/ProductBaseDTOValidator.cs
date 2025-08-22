using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAPIValidation.DTOs;
using FluentValidation;

namespace FluentAPIValidation.Validators
{
    public class ProductBaseDTOValidator<T> : AbstractValidator<T> where T : ProductBaseDTO
    {
        public ProductBaseDTOValidator()
        {
            RuleFor(p => p.SKU)
                .NotEmpty().WithMessage("SKU is required.")
                .Matches("^[A-Z0-9]{8}$").WithMessage("SKU must be 8 uppercase letters or digits.");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .Length(3, 50).WithMessage("Product name must be between 3 and 50 characters.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.")
                .PrecisionScale(8, 2, true).WithMessage("Price must have at most 8 digits in total and 2 decimals.");

            RuleFor(p => p.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");

            RuleFor(p => p.CategoryId)
                .GreaterThan(0).WithMessage("Category ID is required.");

            RuleFor(p => p.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
                .When(p => !string.IsNullOrEmpty(p.Description));

            RuleFor(p => p.Discount)
                .InclusiveBetween(0, 100).WithMessage("Discount must be between 0 and 100.");

            RuleFor(p => p.ManufacturingDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Manufacturing date cannot be in the future.");

            RuleFor(p => p.ExpiryDate)
                .GreaterThan(p => p.ManufacturingDate).WithMessage("Expiry date must be after the manufacturing date.");

            RuleForEach(p => p.Tags).ChildRules(tag =>
            {
                tag.RuleFor(t => t)
                    .NotEmpty().WithMessage("Tag cannot be empty.")
                    .MaximumLength(20).WithMessage("Tag cannot exceed 20 characters.");
            });
        }
    }
}