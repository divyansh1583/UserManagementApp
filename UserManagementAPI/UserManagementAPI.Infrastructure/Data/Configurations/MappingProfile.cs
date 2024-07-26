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
using System.Diagnostics.Metrics;

namespace UserManagementAPI.Infrastructure.Data.Configurations
{
    public class MappingProfile : Profile
    {
        //public MappingProfile()
        //{
        //    CreateMap<DcUser, UserDto>()
        //        .ForMember(dest => dest.Email, opt => opt.Ignore())
        //        .ForMember(dest => dest.Phone, opt => opt.Ignore())
        //        .ForMember(dest => dest.AlternatePhone, opt => opt.Ignore())
        //        .ReverseMap();
        //    CreateMap<DcUserAddress, AddressDto>().ReverseMap();
        //}
        public MappingProfile()
        {

                CreateMap<DcUser, UserDto>()
                // Forward mapping from DcUser to UserDto
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => Encoding.UTF8.GetString(src.Email)))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => Encoding.UTF8.GetString(src.Phone)))
                .ForMember(dest => dest.AlternatePhone, opt => opt.MapFrom(src => src.AlternatePhone != null ? Encoding.UTF8.GetString(src.AlternatePhone) : null))
                .ForMember(dest => dest.DateOfJoining, opt => opt.MapFrom(src => src.DateOfJoining.HasValue ? src.DateOfJoining.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.HasValue ? src.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null))
                .ForMember(dest => dest.PrimaryAddress, opt => opt.MapFrom(src => src.DcUserAddresses.FirstOrDefault(a => a.AddressTypeId == 1)))
                .ForMember(dest => dest.SecondaryAddress, opt => opt.MapFrom(src => src.DcUserAddresses.FirstOrDefault(a => a.AddressTypeId == 2)))
                // Reverse mapping from UserDto to DcUser
                .ReverseMap()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Email)))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Phone)))
                .ForMember(dest => dest.AlternatePhone, opt => opt.MapFrom(src => src.AlternatePhone != null ? Encoding.UTF8.GetBytes(src.AlternatePhone) : null))
                .ForMember(dest => dest.DateOfJoining, opt => opt.MapFrom(src => src.DateOfJoining.HasValue ? DateOnly.FromDateTime(src.DateOfJoining.Value) : (DateOnly?)null))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.HasValue ? DateOnly.FromDateTime(src.DateOfBirth.Value) : (DateOnly?)null))
                .ForMember(dest => dest.DcUserAddresses, opt => opt.Ignore());


            CreateMap<DcUserAddress, AddressDto>()
                .ForMember(dest => dest.AddressTypeName, opt => opt.MapFrom(src => src.AddressType.AddressTypeName))
                .ReverseMap()
                .ForMember(dest => dest.AddressType, opt => opt.Ignore());


        }
    }
}
