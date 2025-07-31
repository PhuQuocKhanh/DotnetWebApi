using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutomapperNullSubstitute.DTOs;
using AutomapperNullSubstitute.Models;

namespace AutomapperNullSubstitute.ProfilesMapping
{
    public class MyMappingComplexTypeProfile : Profile
    {
        public MyMappingComplexTypeProfile()
        {
            // Ánh xạ đơn giản giữa Address và AddressDTO
            CreateMap<AddressComplexType, AddressDTOComplexType>();

            // Cấu hình ánh xạ giữa Employee và EmployeeDTO
            CreateMap<EmployeeComplexType, EmployeeDTOComplexType>()
            // Nếu Address là null, thay thế bằng đối tượng mới với các giá trị mặc định "NA"
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Address ?? new AddressComplexType
                {
                    City = "NA",
                    State = "NA",
                    Country = "NA"
                }))
            // Nếu CreatedBy là null, thay thế bằng "System"
            .ForMember(dest => dest.CreatedBy, opt => opt.NullSubstitute("System"))
            // Nếu CreatedOn là null, thay thế bằng thời điểm hiện tại
            .ForMember(dest => dest.CreatedOn, opt => opt.NullSubstitute(DateTime.Now));
        }
    }
}