using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutomapperNullSubstitute.DTOs;
using AutomapperNullSubstitute.Models;

namespace AutomapperNullSubstitute.ProfilesMapping
{
    public class MyMappingProfile : Profile
    {
        public MyMappingProfile()
        {
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(dest => dest.Address, opt => opt.NullSubstitute("N/A"))
                .ForMember(dest => dest.CreatedBy, opt => opt.NullSubstitute("System"))
                .ForMember(dest => dest.CreatedOn, opt => opt.NullSubstitute(DateTime.Now));
        }
    }
}