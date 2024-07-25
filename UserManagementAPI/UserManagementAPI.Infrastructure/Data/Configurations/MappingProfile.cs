using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;
using UserManagementAPI.Application.DTOs;
using UserManagementAPI.Domain.Entities;

namespace UserManagementAPI.Infrastructure.Data.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DcUser, UserDto>()
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.Phone, opt => opt.Ignore())
                .ForMember(dest => dest.AlternatePhone, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<DcUserAddress, AddressDto>().ReverseMap();
        }
    }
}
