
using CollegeManagementAPI.Infrastructure.Implementation.Services;
using UserManagementAPI.Application.Repositories;
using UserManagementAPI.Application.Services;
using UserManagementAPI.Domain.Common_Models;
using UserManagementAPI.Domain.Entities;

namespace UserManagementAPI.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;

        public UserService(IUserRepository userRepository, TokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        // Service GetAll
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }
        public async Task<ResponseModel> LoginUserAsync(LoginDetails loginDetails)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDetails.Email);

            if (user != null)
            {
                if (user.Password == loginDetails.Password)
                {
                    var jwtToken = _tokenService.GenerateToken(loginDetails);
                    return new ResponseModel { StatusCode = 200, Data = new { user = loginDetails, token = jwtToken }, Message = ResponseMessages.UserFound };
                }
                else
                {
                    return new ResponseModel { StatusCode = 401, Data = null, Message = ResponseMessages.InvaildPassword };
                }
            }
            else
            {
                return new ResponseModel { StatusCode = 404, Data = null, Message = ResponseMessages.UserNotFound };
            }
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
