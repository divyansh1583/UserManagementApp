
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
        Task<ResponseModel> GetAllUsersAsync();
        Task<ResponseModel> GetUserByIdAsync(int id);
        Task<ResponseModel> AddUserAsync(UserDto userDto);
        Task<ResponseModel> UpdateUserAsync(UpdateUserDto updateUserDto);
        Task<ResponseModel> DeleteUserAsync(int id);
        Task<ResponseModel> SendResetPasswordEmailAsync(string email);
        Task<ResponseModel> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<ResponseModel> LoginAsync(LoginDto loginDto);
    }
}
