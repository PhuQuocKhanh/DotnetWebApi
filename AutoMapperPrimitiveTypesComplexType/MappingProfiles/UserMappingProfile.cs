using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperPrimitiveTypesComplexType.DTOs;
using AutoMapperPrimitiveTypesComplexType.Models;

namespace AutoMapperPrimitiveTypesComplexType.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // Mapping từ Entity User (chứa Address phức tạp) sang UserDTO (các thuộc tính primitive)
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Address.State))
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Address.ZipCode));

            // Mapping từ DTO tạo mới (UserCreateDTO) sang Entity User, bao gồm ánh xạ vào object Address
            CreateMap<UserCreateDTO, User>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    Street = src.Street,
                    City = src.City,
                    State = src.State,
                    ZipCode = src.ZipCode
                }));
        }
    }
}