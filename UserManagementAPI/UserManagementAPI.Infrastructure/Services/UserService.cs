using CollegeManagementAPI.Domain.Common_Models;
using CollegeManagementAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Application.Repositories;
using UserManagementAPI.Application.Services;
using UserManagementAPI.Domain.Entities;

namespace UserManagementAPI.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }
        public async Task<ResponseModel> LoginUserAsync(LoginDetails loginDetails)
        {
            return await _userRepository.LoginAsync(loginDetails);
        }

        public async Task<int> AddUserAsync(User user)
        {
            // Here we can add business logic, validation, etc.
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ArgumentException("Email is required");
            }

            if (string.IsNullOrWhiteSpace(user.Password))
            {
                throw new ArgumentException("Password is required");
            }

            // You might want to add more validations here

            // You could also hash the password here before saving
            // user.Password = HashPassword(user.Password);

            return await _userRepository.AddUserAsync(user);
        }

        // Add more methods as needed
    }
}
