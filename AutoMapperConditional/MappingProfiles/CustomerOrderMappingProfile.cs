using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperConditional.DTOs;
using AutoMapperConditional.Models;

namespace AutoMapperConditional.MappingProfiles
{
    public class CustomerOrderMappingProfile : Profile
    {
        public CustomerOrderMappingProfile()
        {
            // 1) Ánh xạ từ Customer -> CustomerDTO
            CreateMap<Customer, CustomerDTO>()
                // Pre-Condition: Chỉ ánh xạ Orders nếu khách hàng đang hoạt động (IsActive = true)
                .ForMember(
                    dest => dest.Orders,
                    opt =>
                    {
                        opt.PreCondition(src => src.IsActive);
                        opt.MapFrom(src => src.Orders);
                    }
                )
                // AfterMap: Nếu tổng chi tiêu > 1000, thêm (VIP) vào tên khách hàng
                .AfterMap((src, dest) =>
                {
                    decimal totalSpending = dest.Orders.Sum(o => o.OrderTotal);
                    if (totalSpending > 1000)
                    {
                        dest.Name += " (VIP)";
                    }
                });

            // 2) Ánh xạ từ Product -> ProductDTO
            CreateMap<Product, ProductDTO>();

            // 3) Ánh xạ từ OrderItem -> OrderItemDTO
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(
                    dest => dest.ProductName,
                    opt =>
                    {
                        // Pre-Condition: Chỉ ánh xạ tên sản phẩm nếu sản phẩm đang còn hàng
                        opt.PreCondition(src => src.Product.IsAvailable);
                        opt.MapFrom(src => src.Product.Name);
                    }
                )
                .ForMember(
                    dest => dest.SubTotal,
                    opt =>
                    {
                        // Condition: Chỉ ánh xạ SubTotal nếu số lượng > 0
                        opt.Condition((src, dest, srcValue, destValue) => src.Quantity > 0);
                        opt.MapFrom(src => src.SubTotal);
                    }
                );

            // 4) Ánh xạ từ Order -> OrderDTO
            CreateMap<Order, OrderDTO>()
                .ForMember(
                    dest => dest.ShippingCost,
                    opt =>
                    {
                        // Condition: Chỉ ánh xạ chi phí vận chuyển nếu đơn đã được giao
                        opt.Condition((src, dest, srcValue, destValue) => src.IsShipped);
                        opt.MapFrom(src => src.ShippingCost);
                    }
                )
                // AfterMap: Làm sạch dữ liệu sau ánh xạ
                .AfterMap((src, dest) =>
                {
                    if (dest.ShippingCost < 0)
                    {
                        dest.ShippingCost = 0;
                    }

                    decimal itemsTotal = dest.OrderItems.Sum(i => i.SubTotal);
                    dest.OrderTotal = itemsTotal < 0 ? 0 : itemsTotal;
                });
        }
    }
}