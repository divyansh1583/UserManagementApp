
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Application.DTOs;
using UserManagementAPI.Application.Interfaces.Services;
using UserManagementAPI.Domain.Common_Models;
using UserManagementAPI.Domain.Entities;

namespace UserManagementAPI.Controllers
{

    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("get")]
        public async Task<ActionResult<ResponseModel>> GetUser([FromBody] int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return new ResponseModel { StatusCode = 404, Message = "User not found" };
            }
            return new ResponseModel { StatusCode = 200, Data = user };
        }

        [HttpGet("getall")]
        public async Task<ActionResult<ResponseModel>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return new ResponseModel { StatusCode = 200, Data = new{ User=users } };
        }

        [HttpPost("add")]
        public async Task<ActionResult<ResponseModel>> AddUser(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseModel { StatusCode = 400, Message = "Invalid request" };
            }

            var userId = await _userService.AddUserAsync(userDto);
            if (userId >= 3)
            {
                return new ResponseModel { StatusCode = 201, Data = new { UserId = userId    }, Message = "User added successfully" };
            }
            return new ResponseModel { StatusCode = 500, Message = "Failed to add user" };
        }

        //[HttpPut("update")]
        //public async Task<ActionResult<ResponseModel>> UpdateUser(UserDto userDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return new ResponseModel { StatusCode = 400, Message = "Invalid request" };
        //    }

        //    var result = await _userService.UpdateUserAsync(userDto);
        //    if (!result)
        //    {
        //        return new ResponseModel { StatusCode = 404, Message = "User not found" };
        //    }
        //    return new ResponseModel { StatusCode = 200, Message = "User updated successfully" };
        //}

        //[HttpDelete("delete")]
        //public async Task<ActionResult<ResponseModel>> DeleteUser([FromBody] int id)
        //{
        //    var result = await _userService.DeleteUserAsync(id);
        //    if (!result)
        //    {
        //        return new ResponseModel { StatusCode = 404, Message = "User not found" };
        //    }
        //    return new ResponseModel { StatusCode = 200, Message = "User deleted successfully" };
        //}
    }
}
