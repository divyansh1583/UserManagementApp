using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Application.DTOs;
using UserManagementAPI.Application.Interfaces.Repositories;
using UserManagementAPI.Application.Interfaces.Services;
using UserManagementAPI.Domain.Common_Models;

namespace UserManagementAPI.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public AuthService(
            IUserRepository userRepository,
            ITokenService tokenService,
            IEmailService emailService,
            IConfiguration configuration,
            IMapper mapper
            )
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _emailService = emailService;
            _configuration = configuration;
            _mapper = mapper;
        }

        //public async Task<ResponseModel> LoginAsync(LoginDto loginDto)
        //{
        //    var user = await _userRepository.GetByEmailAsync(loginDto.Email);

        //    if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
        //    {
        //        return new ResponseModel { StatusCode = 401, Message = "Invalid credentials" };
        //    }
        //    var userDto=_mapper.Map<UserDto>(user);
        //    if ((bool)!userDto.IsActive)
        //    {
        //        await SendActivationEmailAsync(userDto);
        //        return new ResponseModel { StatusCode = 403, Message = "Account is inactive. An activation link has been sent to your email." };
        //    }

        //    UserDto userDto = _mapper.Map<UserDto>(user);
        //    var token = _tokenService.GenerateToken(userDto);
        //    return new ResponseModel { StatusCode = 200, Message = "Login successful", Data = new { Token = token } };
        //}

        //private async Task SendActivationEmailAsync(UserDto user)
        //{
        //    user.PasswordResetToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        //    user.PasswordResetTokenExpiry = DateTime.Now.AddHours(24);

        //    await _userRepository.UpdateAsync(user);

        //    string from = _configuration["EmailSettings:From"] ?? "error@gmail.com";
        //    var activationLink = $"http://yourfrontendurl.com/activate?email={user.Email}&token={user.PasswordResetToken}";
        //    var emailModel = new EmailModel(user.Email, "Activate Your Account", $" Here is your activation link: \nEmail: {user.Email} \nLink: {activationLink}");
        //    _emailService.SendEmail(emailModel);
        //}

        //public async Task<ResponseModel> ActivateAccountAsync(string email, string token)
        //{
        //    var user = await _userRepository.GetByEmailAsync(email);

        //    if (user == null)
        //    {
        //        return new ResponseModel { StatusCode = 404, Message = "User doesn't exist" };
        //    }

        //    if ((bool)user.IsActive)
        //    {
        //        return new ResponseModel { StatusCode = 400, Message = "Account is already active" };
        //    }

        //    if (user.PasswordResetToken != token)
        //    {
        //        return new ResponseModel { StatusCode = 400, Message = "Invalid activation token" };
        //    }

        //    if (user.PasswordResetTokenExpiry < DateTime.Now)
        //    {
        //        return new ResponseModel { StatusCode = 400, Message = "Activation token has expired. Please request a new one." };
        //    }

        //    user.IsActive = true;
        //    user.PasswordResetToken = null;
        //    user.PasswordResetTokenExpiry = null;

        //    await _userRepository.UpdateAsync(user);

        //    return new ResponseModel { StatusCode = 200, Message = "Account activated successfully" };
        //}
        public async Task<ResponseModel> SendResetPasswordEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return new ResponseModel { StatusCode = 404, Message = "Email doesn't exist" };
            }

            user.PasswordResetToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            user.PasswordResetTokenExpiry = DateTime.Now.AddMinutes(15);

            string from = _configuration["EmailSettings:From"] ?? "error@gmail.com";
            var emailModel = new EmailModel(email, "Reset Password!!", EmailBody.EmailStringBody(email, user.PasswordResetToken));
            _emailService.SendEmail(emailModel);

            await _userRepository.UpdateAsync(user);

            return new ResponseModel { StatusCode = 200, Message = "Email sent!" };
        }

        public async Task<ResponseModel> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userRepository.GetByEmailAsync(resetPasswordDto.Email);

            if (user == null)
            {
                return new ResponseModel { StatusCode = 404, Message = "User doesn't exist" };
            }
            if (user.PasswordResetToken != resetPasswordDto.EmailToken)
            {
                return new ResponseModel { StatusCode = 400, Message = "Invalid Password Reset Token " };
            }
            if (user.PasswordResetTokenExpiry < DateTime.Now)
            {
                return new ResponseModel { StatusCode = 400, Message = "Time to reset password has expired. Try Again!" };
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;

            await _userRepository.UpdateAsync(user);

            return new ResponseModel { StatusCode = 200, Message = "Password reset successfully" };
        }
        public async Task<ResponseModel> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            var user = await _userRepository.GetByIdAsync(changePasswordDto.UserId);
            if (user == null)
            {
                return new ResponseModel { StatusCode = 404, Message = "User not found" };
            }

            if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.Password))
            {
                return new ResponseModel { StatusCode = 400, Message = "Current password is incorrect" };
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
            var updated = await _userRepository.UpdateAsync(user);

            if (updated)
            {
                return new ResponseModel { StatusCode = 200, Message = "Password changed successfully" };
            }
            return new ResponseModel { StatusCode = 500, Message = "Failed to change password" };
        }
    }
}
