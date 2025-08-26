using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using ValidateNestedComplex.DTOs;

namespace ValidateNestedComplex.Validator
{
    public class AddressDTOValidator : AbstractValidator<AddressDTO>
    {
        public AddressDTOValidator()
        {
            RuleFor(a => a.City)
                .NotEmpty().WithMessage("Thành phố là bắt buộc.")
                .MaximumLength(100).WithMessage("Thành phố không được vượt quá 100 ký tự.");
            RuleFor(a => a.State)
                .NotEmpty().WithMessage("Tiểu bang là bắt buộc.")
                .MaximumLength(100).WithMessage("Tiểu bang không được vượt quá 100 ký tự.");
            RuleFor(a => a.ZipCode)
                .NotEmpty().WithMessage("Mã ZipCode là bắt buộc.")
                .Matches(@"^\d{5,6}$").WithMessage("ZipCode phải có 5 hoặc 6 chữ số.");
        }
    }
}