using System.Security.Cryptography;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using UserManagementAPI.Application.DTOs;
using UserManagementAPI.Application.Interfaces.Repositories;
using UserManagementAPI.Application.Interfaces.Services;
using UserManagementAPI.Domain.Common_Models;
using UserManagementAPI.Domain.Entities;

namespace UserManagementAPI.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEncryptionService encryptionService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IEncryptionService encryptionService,
            ITokenService tokenService,
            IConfiguration configuration,
            IEmailService emailService
            )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            encryptionService = encryptionService;
            _tokenService = tokenService;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<ResponseModel> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return new ResponseModel { StatusCode = 200, Data = userDtos };
        }

        public async Task<ResponseModel> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return new ResponseModel { StatusCode = 404, Message = "User not found" };
            }
            var userDto = _mapper.Map<UserDto>(user);
            return new ResponseModel { StatusCode = 200, Data = userDto };
        }

        public async Task<ResponseModel> AddUserAsync(UserDto userDto)
        {
            DcUser user = _mapper.Map<DcUser>(userDto);
            var userChanges = await _userRepository.AddAsync(user);
            if (userChanges >= 3)
            {
                return new ResponseModel { StatusCode = 201, Message = "User added successfully" };
            }
            return new ResponseModel { StatusCode = 500, Message = "Failed to add user" };
        }

        public async Task<ResponseModel> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(updateUserDto.UserId);
            if (existingUser == null)
            {
                return new ResponseModel { StatusCode = 404, Message = "User not found" };
            }

            _mapper.Map(updateUserDto, existingUser);

            var userUpdated = await _userRepository.UpdateAsync(existingUser);
            if (userUpdated)
            {
                return new ResponseModel { StatusCode = 200, Message = "User updated successfully" };
            }
            return new ResponseModel { StatusCode = 500, Message = "Failed to update user" };
        }

        public async Task<ResponseModel> DeleteUserAsync(int id)
        {
            var userDeleted = await _userRepository.DeleteAsync(id);
            if (userDeleted)
            {
                return new ResponseModel { StatusCode = 200, Message = "User deleted successfully" };
            }
            return new ResponseModel { StatusCode = 500, Message = "Failed to delete user" };
        }

        public async Task<ResponseModel> SendResetPasswordEmailAsync(string email)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => encryptionService.Decrypt(u.Email) == email );
            if (user == null)
            {
                return new ResponseModel { StatusCode = 404, Message = "Email doesn't exist" };
            }

            user.PasswordResetToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            user.PasswordResetTokenExpiry = DateTime.Now.AddMinutes(15);

            string from = _configuration["EmailSettings:From"];
            var emailModel = new EmailModel(email, "Reset Password!!", EmailBody.EmailStringBody(email, user.PasswordResetToken));
            _emailService.SendEmail(emailModel);

            await _userRepository.UpdateAsync(user);

            return new ResponseModel { StatusCode = 200, Message = "Email sent!" };
        }

        public async Task<ResponseModel> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => encryptionService.Decrypt(u.Email) == resetPasswordDto.Email);
            if (user == null)
            {
                return new ResponseModel { StatusCode = 404, Message = "User doesn't exist" };
            }

            if (user.PasswordResetToken != resetPasswordDto.EmailToken || user.PasswordResetTokenExpiry < DateTime.Now)
            {
                return new ResponseModel { StatusCode = 400, Message = "Invalid reset link" };
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;

            await _userRepository.UpdateAsync(user);

            return new ResponseModel { StatusCode = 200, Message = "Password reset successfully" };
        }

        public async Task<ResponseModel> LoginAsync(LoginDto loginDto)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => encryptionService.Decrypt(u.Email) == loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return new ResponseModel { StatusCode = 401, Message = "Invalid credentials" };
            }

            var token = _tokenService.GenerateToken();
            return new ResponseModel { StatusCode = 200, Message = "Login successful", Data = new { Token = token } };
        }
    }
}