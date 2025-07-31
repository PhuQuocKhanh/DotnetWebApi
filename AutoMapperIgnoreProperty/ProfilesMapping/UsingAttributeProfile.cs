using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperIgnoreProperty.DTOs;
using AutoMapperIgnoreProperty.Extensions;
using AutoMapperIgnoreProperty.Models;

namespace AutoMapperIgnoreProperty.ProfilesMapping
{
    public class UsingAttributeProfile : Profile
    {
        public UsingAttributeProfile()
        {
            CreateMap<UserUsingAttribute, UserDTO>()
                .IgnoreNoMap();
        }
    }
}