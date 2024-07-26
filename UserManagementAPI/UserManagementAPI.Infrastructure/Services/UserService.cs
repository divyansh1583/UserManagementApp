
using AutoMapper;
using CollegeManagementAPI.Infrastructure.Implementation.Services;
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
        private readonly IEncryptionService _encryptionService;

        public UserService(IUserRepository userRepository, IMapper mapper, IEncryptionService encryptionService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _encryptionService = encryptionService;
        }
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<int> AddUserAsync(UserDto userDto)
        {
            DcUser user = _mapper.Map<DcUser>(userDto);

            //Encrypting Email,Phone, AlternatePhone 
            user.Email = _encryptionService.Encrypt(userDto.Email);
            user.Phone = _encryptionService.Encrypt(userDto.Phone);
            user.AlternatePhone = _encryptionService.Encrypt(userDto.AlternatePhone);

            //Hashing Password
            user.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            var userId = await _userRepository.AddAsync(user);

            return userId;
        }


        public Task<bool> UpdateUserAsync(UserDto userDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
