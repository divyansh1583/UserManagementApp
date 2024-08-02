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
using System.Web;
using PasswordGenerator;

namespace UserManagementAPI.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
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

            var pwd = new Password(8).IncludeLowercase().IncludeUppercase().IncludeSpecial().IncludeNumeric();
            var password = pwd.Next();
            userDto.Password = password;
            DcUser user = _mapper.Map<DcUser>(userDto);
            var userChanges = await _userRepository.AddAsync(user);
            if (userChanges >= 2)
            {
                var emailModel = new EmailModel(userDto.Email, "New User Account - Login Credentials", $"Congratulations, your new user account has been created! Your login credentials are: \nEmail: {userDto.Email} \nPassword: {password}");
                _emailService.SendEmail(emailModel);
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
            var imagePath = _environment.WebRootPath + relativePath;
            return new ResponseModel { StatusCode = 200, Message = "Image uploaded successfully", Data = relativePath };
        }
    }
}