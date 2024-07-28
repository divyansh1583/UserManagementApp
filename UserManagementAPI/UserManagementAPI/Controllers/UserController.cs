using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Application.DTOs;
using UserManagementAPI.Application.Interfaces.Services;
using UserManagementAPI.Domain.Common_Models;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getall")]
        public async Task<ActionResult<ResponseModel>> GetAllUsers()
        {
            return await _userService.GetAllUsersAsync();
        }

        [HttpGet("getById")]
        public async Task<ActionResult<ResponseModel>> GetUser([FromBody] int id)
        {
            return await _userService.GetUserByIdAsync(id);
        }

        [HttpPost("add")]
        public async Task<ActionResult<ResponseModel>> AddUser(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseModel { StatusCode = 400, Message = "Invalid request" };
            }
            return await _userService.AddUserAsync(userDto);
        }

        [HttpPut("update")]
        public async Task<ActionResult<ResponseModel>> UpdateUser(UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseModel { StatusCode = 400, Message = "Invalid request" };
            }
            return await _userService.UpdateUserAsync(updateUserDto);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<ResponseModel>> DeleteUser(int id)
        {
            return await _userService.DeleteUserAsync(id);
        }

        [HttpPost("send-reset-email/{email}")]
        public async Task<ActionResult<ResponseModel>> SendResetPasswordEmail(string email)
        {
            return await _userService.SendResetPasswordEmailAsync(email);
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<ResponseModel>> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            return await _userService.ResetPasswordAsync(resetPasswordDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseModel>> Login(LoginDto loginDto)
        {
            return await _userService.LoginAsync(loginDto);
        }
    }
}