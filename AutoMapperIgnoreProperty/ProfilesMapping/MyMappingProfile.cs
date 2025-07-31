using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperIgnoreProperty.DTOs;
using AutoMapperIgnoreProperty.Models;

namespace AutoMapperIgnoreProperty.ProfilesMapping
{
    public class MyMappingProfile : Profile
    {
        public MyMappingProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityToken, opt => opt.Ignore());
        }
    }
}