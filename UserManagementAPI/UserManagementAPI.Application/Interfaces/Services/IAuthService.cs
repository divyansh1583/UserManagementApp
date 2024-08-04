using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Application.DTOs;
using UserManagementAPI.Domain.Common_Models;

namespace UserManagementAPI.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ResponseModel> LoginAsync(LoginDto loginDto);
        Task<ResponseModel> SendResetPasswordEmailAsync(string email);
        Task<ResponseModel> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<ResponseModel> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
        //Task<ResponseModel> ActivateAccountAsync(string email, string token);
    }
}
