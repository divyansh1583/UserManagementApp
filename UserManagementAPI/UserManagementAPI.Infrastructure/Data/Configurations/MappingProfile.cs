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
using UserManagementAPI.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using UserManagementAPI.Infrastructure.Services;

namespace UserManagementAPI.Infrastructure.Data.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Move the mapping configuration to a separate method
            ConfigureMappings(null);
        }

        public MappingProfile(IEncryptionService encryptionService)
        {
            ConfigureMappings(encryptionService);
        }

        private void ConfigureMappings(IEncryptionService encryptionService)
        {
            if (encryptionService == null)
            {
                throw new ArgumentNullException(nameof(encryptionService));
            }
            CreateMap<UserDto, DcUser>()
            // Forward mapping from UserDto to DcUser
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => encryptionService.Encrypt(src.Email)))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => encryptionService.Encrypt(src.Phone)))
            .ForMember(dest => dest.AlternatePhone, opt => opt.MapFrom(src => src.AlternatePhone != null ? encryptionService.Encrypt(src.AlternatePhone) : null))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)))
            .ForMember(dest => dest.DateOfJoining, opt => opt.MapFrom(src => src.DateOfJoining.HasValue ? DateOnly.FromDateTime(src.DateOfJoining.Value) : (DateOnly?)null))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.HasValue ? DateOnly.FromDateTime(src.DateOfBirth.Value) : (DateOnly?)null))
            .ForMember(dest => dest.DcUserAddresses, opt => opt.MapFrom(src => src.Addresses))
            .AfterMap((src, dest) => dest.CreatedBy = src.FirstName)
            .AfterMap((src, dest) => dest.CreatedDate = DateTime.Now)
            .AfterMap((src, dest) => dest.UpdatedBy = null)
            .AfterMap((src, dest) => dest.UpdatedDate = null)
            
            // Reverse mapping from DcUser to UserDto
            .ReverseMap()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => encryptionService.Decrypt(src.Email)))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => encryptionService.Decrypt(src.Phone)))
            .ForMember(dest => dest.AlternatePhone, opt => opt.MapFrom(src => src.AlternatePhone != null ? encryptionService.Decrypt(src.AlternatePhone) : null))
            .ForMember(dest => dest.Password, opt => opt.Ignore()) // Ignore password when reversing
            .ForMember(dest => dest.DateOfJoining, opt => opt.MapFrom(src => src.DateOfJoining.HasValue ? src.DateOfJoining.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.HasValue ? src.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null))
            .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.DcUserAddresses));

            CreateMap<UpdateUserDto, DcUser>()
                        .ForMember(dest => dest.Phone, opt =>
                        {
                            opt.Condition(src => src.Phone != null);
                            opt.MapFrom(src => encryptionService.Encrypt(src.Phone));
                        })
                        .ForMember(dest => dest.AlternatePhone, opt =>
                        {
                            opt.Condition(src => src.AlternatePhone != null);
                            opt.MapFrom(src => encryptionService.Encrypt(src.AlternatePhone));
                        })
                        .ForMember(dest => dest.DateOfJoining, opt =>
                        {
                            opt.Condition(src => src.DateOfJoining.HasValue);
                            opt.MapFrom(src => DateOnly.FromDateTime(src.DateOfJoining.Value));
                        })
                        .ForMember(dest => dest.DateOfBirth, opt =>
                        {
                            opt.Condition(src => src.DateOfBirth.HasValue);
                            opt.MapFrom(src => DateOnly.FromDateTime(src.DateOfBirth.Value));
                        })
                        .AfterMap((src, dest) => dest.UpdatedBy = src.FirstName)
                        .AfterMap((src, dest) => dest.UpdatedDate = DateTime.Now)

                        .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<AddressDto, DcUserAddress>()
                .ReverseMap();
        }
    }
}
