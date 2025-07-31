using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperIgnoreProperty.DTOs;
using AutoMapperIgnoreProperty.Models;

namespace AutoMapperIgnoreProperty.Extensions
{
    public class IgnoreAllNonExistingExtensions : Profile
    {
        public IgnoreAllNonExistingExtensions()
        {
            CreateMap<EmployeeIgnoreAllNonExisting, EmployeeDTOIgnoreAllNonExisting>(MemberList.None);
        }
    }
}