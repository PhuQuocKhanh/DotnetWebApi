using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAPIConditionValidator.Data;
using FluentAPIConditionValidator.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FluentAPIConditionValidator.Validators
{
    public class OrderDTOValidator : AbstractValidator<OrderDTO>
    {
        public OrderDTOValidator(ECommerceDbContext context)
        {
            // ----- Validations chung -----
            // Xác thực CustomerId được cung cấp và tồn tại trong cơ sở dữ liệu
            RuleFor(order => order.CustomerId)
                .NotEmpty().WithMessage("CustomerId is required.")
                .MustAsync(async (customerId, cancellationToken) =>
                {
                    return await context.Customers.AnyAsync(c => c.CustomerId == customerId, cancellationToken);
                })
                .WithMessage("Invalid Customer.");
            // Xác thực PaymentMode được cung cấp và là một trong các giá trị cho phép
            RuleFor(order => order.PaymentMode)
                .NotEmpty().WithMessage("Payment mode is required.")
                .Must(mode => new List<string> { "CreditCard", "UPI", "Cash" }.Contains(mode))
                .WithMessage("Invalid payment mode. Allowed values: CreditCard, UPI, Cash.");
            // Số tiền đơn hàng phải lớn hơn không
            RuleFor(order => order.OrderAmount)
                .GreaterThan(0)
                .WithMessage("Order amount must be greater than zero.");
            // Địa chỉ giao hàng là bắt buộc
            RuleFor(order => order.ShippingAddress)
                .NotEmpty()
                .WithMessage("Shipping address is required.");

            // ----- Validations có điều kiện -----
            // Nếu PaymentMode là UPI, thì UPIId phải được cung cấp
            RuleFor(order => order.UPIId)
                .NotEmpty()
                .When(order => order.PaymentMode == "UPI")
                .WithMessage("UPIId is required for UPI payments.");

            // Một cách khác để sử dụng phương thức When
            When(order => order.PaymentMode == "UPI", () =>
            {
                RuleFor(order => order.UPIId)
                    .NotEmpty()
                    .WithMessage("UPIId is required for UPI payments.");
            });

            // Nếu PaymentMode là CreditCard, thì CreditCardNumber phải được cung cấp
            When(order => order.PaymentMode == "CreditCard", () =>
            {
                RuleFor(order => order.CreditCardNumber)
                    .NotEmpty()
                    .WithMessage("CreditCardNumber is required for Credit Card payments.");
            });

            // Đối với các thanh toán không dùng tiền mặt, đảm bảo Discount nằm trong khoảng cho phép (10% đến 30%).
            // Sử dụng 'Unless' để bỏ qua quy tắc này nếu PaymentMode là "Cash"
            RuleFor(order => order.Discount)
                .InclusiveBetween(10, 30)
                .Unless(order => order.PaymentMode == "Cash")
                .WithMessage("Discount must be between 10% and 30% for non-cash payments.");

            // Một cách khác để sử dụng phương thức Unless
            // Unless(order => order.PaymentMode == "Cash", () =>
            // {
            //     RuleFor(order => order.Discount)
            //         .InclusiveBetween(10, 30)
            //         .WithMessage("Discount must be between 10% and 30% for non-cash payments.");
            // });
        }
    }
}