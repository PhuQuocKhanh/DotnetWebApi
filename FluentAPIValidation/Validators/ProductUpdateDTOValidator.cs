using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAPIValidation.DTOs;
using FluentValidation;

namespace FluentAPIValidation.Validators
{
 // Validator cho ProductUpdateDTO, chứa các rule khi update sản phẩm
    public class ProductUpdateDTOValidator : AbstractValidator<ProductUpdateDTO>
    {
        public ProductUpdateDTOValidator()
        {
            // ProductId: > 0
            RuleFor(p => p.ProductId)
                .GreaterThan(0).WithMessage("ProductId Must be Greater than 0");

            // SKU: bắt buộc, regex, 8 ký tự in hoa hoặc số
            RuleFor(p => p.SKU)
                .NotEmpty().WithMessage("SKU is required.")
                .Matches("^[A-Z0-9]{8}$").WithMessage("SKU must be 8 characters long and contain only uppercase letters and digits.");

            // Name: bắt buộc, 3–50 ký tự
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .Length(3, 50).WithMessage("Product name must be between 3 and 50 characters.");

            // Price: > 0, tối đa 8 số và 2 số thập phân
            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.")
                .PrecisionScale(8, 2, true).WithMessage("Price must have at most 8 digits in total and 2 decimals.");

            // Stock: >= 0
            RuleFor(p => p.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");

            // CategoryId: > 0
            RuleFor(p => p.CategoryId)
                .GreaterThan(0).WithMessage("Category ID is required.");

            // Description: nếu có thì tối đa 500 ký tự
            RuleFor(p => p.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
                .When(p => !string.IsNullOrEmpty(p.Description));

            // Discount: trong khoảng 0–100
            RuleFor(p => p.Discount)
                .InclusiveBetween(0, 100).WithMessage("Discount must be between 0 and 100.");

            // ManufacturingDate: không lớn hơn hiện tại
            RuleFor(p => p.ManufacturingDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Manufacturing date cannot be in the future.");

            // ExpiryDate: phải > ManufacturingDate
            RuleFor(p => p.ExpiryDate)
                .GreaterThan(p => p.ManufacturingDate).WithMessage("Expiry date must be after the manufacturing date.");

            // Tags: mỗi tag không được rỗng, tối đa 20 ký tự
            RuleForEach(p => p.Tags).ChildRules(tag =>
            {
                tag.RuleFor(t => t)
                    .NotEmpty().WithMessage("Tag cannot be empty.")
                    .MaximumLength(20).WithMessage("Tag cannot exceed 20 characters.");
            });
        }
    }
}