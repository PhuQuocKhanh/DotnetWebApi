using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAPIAsyncValidators.Data;
using FluentAPIAsyncValidators.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FluentAPIAsyncValidators.Validators
{
    // Validator for ProductDTO using FluentValidation.
    public class ProductDTOValidator : AbstractValidator<ProductDTO>
    {
        private readonly ECommerceDbContext _context;
        // Inject the DbContext for performing async validations.
        public ProductDTOValidator(ECommerceDbContext context)
        {
            _context = context;
            // Validate the 'Name' property:
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .Length(3, 100).WithMessage("Product name must be between 3 and 100 characters.")
                .MustAsync(BeUniqueNameAsync).WithMessage("Product name must be unique.");
            // Validate the 'SKU' property:
            RuleFor(x => x.SKU)
                .NotEmpty().WithMessage("SKU is required.")
                .Length(3, 20).WithMessage("SKU must be between 3 and 20 characters.")
                .MustAsync(BeUniqueSKUAsync).WithMessage("SKU must be unique.");
            // Validate the 'Price' property:
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");
            // Validate the 'Stock' property:
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");
            // Validate the 'CategoryId' property:
            RuleFor(x => x.CategoryId)
                .MustAsync(CategoryExistsAsync).WithMessage("Category does not exist.");
            // Validate the 'Discount' property:
            RuleFor(x => x.Discount)
                .InclusiveBetween(0, 50).WithMessage("Discount must be between 0 and 50%.")
                .MustAsync(IsValidDiscountBasedOnRuleAsync).WithMessage("Discount is not valid for the given product price.");
        }
        // Checks asynchronously that the product name is unique in the database.
        private async Task<bool> BeUniqueNameAsync(string productName, CancellationToken cancellationToken)
        {
            return !await _context.Products.AsNoTracking()
                .AnyAsync(p => p.Name == productName, cancellationToken);
        }
        // Checks asynchronously that the SKU is unique in the database.
        private async Task<bool> BeUniqueSKUAsync(string sku, CancellationToken cancellationToken)
        {
            return !await _context.Products.AsNoTracking()
                .AnyAsync(p => p.SKU == sku, cancellationToken);
        }
        // Checks asynchronously that the provided CategoryId exists.
        private async Task<bool> CategoryExistsAsync(int categoryId, CancellationToken cancellationToken)
        {
            return await _context.Categories.AsNoTracking()
                .AnyAsync(c => c.CategoryId == categoryId, cancellationToken);
        }
        // Asynchronously verifies that the discount is valid based on applicable discount rules.
        private async Task<bool> IsValidDiscountBasedOnRuleAsync(ProductDTO product, decimal discount, CancellationToken cancellationToken)
        {
            // Retrieve the most appropriate discount rule for the given product price.
            var discountRule = await _context.DiscountRules
                .AsNoTracking()
                .Where(rule => product.Price >= rule.MinimumPrice)
                .OrderByDescending(rule => rule.MinimumPrice)
                .FirstOrDefaultAsync(cancellationToken);
            if (discountRule == null)
            {
                // No discount rule applies if the product price is below all defined thresholds.
                return false;
            }
            // The discount is valid if it does not exceed the maximum discount allowed by the rule.
            return discount <= discountRule.MaximumDiscount;
        }
    }
}