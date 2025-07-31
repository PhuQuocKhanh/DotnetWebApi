using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperComplexMappingDemo.DTOs;
using AutoMapperComplexMappingDemo.Models;

namespace AutoMapperComplexMappingDemo.MappingProfiles
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            // Cấu hình ánh xạ từ Order entity sang OrderDTO
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => $"{src.Customer.FirstName} {src.Customer.LastName}"))
                .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.Customer.Email))
                .ForMember(dest => dest.CustomerPhoneNumber, opt => opt.MapFrom(src => src.Customer.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Customer.Address))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));

            // Ánh xạ từ OrderItem sang OrderItemDTO
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Product.Price * src.Quantity));

            // Ánh xạ từ Address sang AddressDTO (các thuộc tính giống nhau nên không cần cấu hình cụ thể)
            CreateMap<Address, AddressDTO>();

            // Ánh xạ từ OrderCreateDTO sang Order (dùng khi tạo đơn hàng)
            CreateMap<OrderCreateDTO, Order>()
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateTime.Now)) // Gán ngày tạo là thời gian hiện tại
                .ForMember(dest => dest.Amount, opt => opt.Ignore()) // Không ánh xạ vì sẽ tính toán riêng
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items));

            // Ánh xạ từ OrderItemCreateDTO sang OrderItem
            CreateMap<OrderItemCreateDTO, OrderItem>(); // Không cần cấu hình nếu trùng tên thuộc tính
        }
    }
}