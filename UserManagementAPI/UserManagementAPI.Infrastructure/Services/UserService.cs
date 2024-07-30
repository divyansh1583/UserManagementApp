using System.Security.Cryptography;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using UserManagementAPI.Application.DTOs;
using UserManagementAPI.Application.Interfaces.Repositories;
using UserManagementAPI.Application.Interfaces.Services;
using UserManagementAPI.Domain.Common_Models;
using UserManagementAPI.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using Microsoft.AspNetCore.Http;

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
        private readonly IEncryptionService _encryptionService;
        private readonly IWebHostEnvironment _environment;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IEncryptionService encryptionService,
            ITokenService tokenService,
            IConfiguration configuration,
            IEmailService emailService,
            IWebHostEnvironment environment
            )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _encryptionService = encryptionService;
            _tokenService = tokenService;
            _configuration = configuration;
            _emailService = emailService;
            _environment = environment;
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

            var userExist = await _userRepository.GetByEmailAsync(userDto.Email);
            if (userExist != null)
            {
                return new ResponseModel { StatusCode = 409, Message = "User Already Exists!" };
            }
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
            var user = await _userRepository.GetByEmailAsync(email);
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
            var user = await _userRepository.GetByEmailAsync(resetPasswordDto.Email);

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
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return new ResponseModel { StatusCode = 401, Message = "Invalid credentials" };
            }

            var token = _tokenService.GenerateToken();
            return new ResponseModel { StatusCode = 200, Message = "Login successful", Data = new { Token = token } };
        }
        public async Task<ResponseModel> UploadUserImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new ResponseModel { StatusCode = 400, Message = "No file uploaded" };
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var relativePath = "/uploads/" + uniqueFileName;
            return new ResponseModel { StatusCode = 200, Message = "Image uploaded successfully", Data = relativePath };
        }
    }
}