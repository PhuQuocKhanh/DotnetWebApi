using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ValidateNestedComplex.Data;
using ValidateNestedComplex.DTOs;

namespace ValidateNestedComplex.Validator
{
    public class OrderDTOValidator : AbstractValidator<OrderDTO>
    {
        // Trường private để giữ instance của database context
        private readonly ECommerceDbContext _context;
        // Constructor chấp nhận ECommerceDbContext qua dependency injection
        public OrderDTOValidator(ECommerceDbContext context)
        {
            // Gán instance DbContext được truyền vào cho trường private
            _context = context; 
            // -------------------------
            // Xác thực thuộc tính CustomerId:
            // -------------------------
            RuleFor(o => o.CustomerId)
                .GreaterThan(0) // Đảm bảo CustomerId lớn hơn 0
                .WithMessage("CustomerId phải lớn hơn 0.")
                .MustAsync(async (customerId, cancellationToken) =>
                {
                    // Kiểm tra bất đồng bộ xem có khách hàng nào với CustomerId đã cho tồn tại trong cơ sở dữ liệu không
                    return await _context.Customers.AnyAsync(u => u.Id == customerId, cancellationToken);
                })
                .WithMessage("Khách hàng không hợp lệ"); // Cung cấp thông báo lỗi nếu khách hàng không tồn tại
            // -------------------------
            // Xác thực ShippingAddressId khi nó được cung cấp (tức là khi lớn hơn 0):
            // -------------------------
            RuleFor(o => o.ShippingAddressId)
                .MustAsync(async (dto, shippingAddressId, cancellationToken) =>
                {
                    // Kiểm tra nếu ShippingAddressId được cung cấp và hợp lệ (> 0)
                    if (shippingAddressId.HasValue && shippingAddressId.Value > 0)
                    {
                        // Xác thực rằng địa chỉ tồn tại trong DB và thuộc về khách hàng được chỉ định bởi dto.CustomerId
                        return await _context.Addresses.AnyAsync(
                            a => a.Id == shippingAddressId.Value && a.CustomerId == dto.CustomerId, cancellationToken);
                    }
                    // Nếu ShippingAddressId là null hoặc 0, bỏ qua kiểm tra này bằng cách trả về true
                    return true;
                })
                .WithMessage("ShippingAddressId không hợp lệ cho Khách hàng đã chỉ định.")
                .When(o => o.ShippingAddressId.HasValue && o.ShippingAddressId.Value > 0);
        
            // -------------------------
            // Xác thực thuộc tính NewAddress khi ShippingAddressId không được cung cấp hoặc bằng 0:
            // -------------------------
            RuleFor(o => o.NewAddress)
                .NotNull() // Đảm bảo NewAddress không phải là null trong trường hợp này
                .WithMessage("Chi tiết địa chỉ mới phải được cung cấp khi ShippingAddressId không được cung cấp hoặc bằng 0.")
                .When(o => !o.ShippingAddressId.HasValue || o.ShippingAddressId.Value == 0)
                // Sau khi xác nhận NewAddress không null, sử dụng DependentRules để xác thực các thuộc tính của nó bằng validator riêng
                .DependentRules(() =>
                {
                    RuleFor(o => o.NewAddress!)
                        // Sử dụng SetValidator để đính kèm AddressDTOValidator vào thuộc tính NewAddress
                        .SetValidator(new AddressDTOValidator());
                });
            // -------------------------
            // Xác thực tập hợp OrderItems:
            // -------------------------
            RuleFor(o => o.OrderItems)
                .NotEmpty() // Đảm bảo tập hợp OrderItems không rỗng
                .WithMessage("Đơn hàng phải có ít nhất một mặt hàng.")
                // Sử dụng ForEach để áp dụng validator cho từng mục trong tập hợp
                .ForEach(oi => oi.SetValidator(new OrderItemDTOValidator()));
        }
    }
}