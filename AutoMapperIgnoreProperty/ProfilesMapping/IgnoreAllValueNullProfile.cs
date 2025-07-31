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
    public class IgnoreAllValueNullProfile : Profile
    {
        public IgnoreAllValueNullProfile()
        {
            CreateMap<ProductDTOIgnoreAllValueNull, ProductIgnoreAllValueNull>().IgnoreAllNull();
        }
    }
}