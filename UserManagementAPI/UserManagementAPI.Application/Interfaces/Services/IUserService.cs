
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Application.DTOs;
using UserManagementAPI.Domain.Common_Models;
using UserManagementAPI.Domain.Entities;

namespace UserManagementAPI.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(int id);
        Task<int> AddUserAsync(DcUser userDto);
    }
}
