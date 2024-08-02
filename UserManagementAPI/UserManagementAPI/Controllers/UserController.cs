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
        IWebHostEnvironment _webHostEnvironment;

        public UsersController(IUserService userService, IWebHostEnvironment webHostEnvironment)
        {
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("getall")]
        public async Task<ActionResult<ResponseModel>> GetAllUsers()
        {
            return await _userService.GetAllUsersAsync();
        }

        [HttpGet("getById")]
        public async Task<ActionResult<ResponseModel>> GetUser(int id)
        {
            return await _userService.GetUserByIdAsync(id);
        }

        [HttpPost("add")]
        public async Task<ActionResult<ResponseModel>> AddUser([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel { StatusCode = 400, Message = "Invalid request", Data = ModelState });
            }
            return await _userService.AddUserAsync(userDto);
        }

        [HttpPut("update")]
        public async Task<ActionResult<ResponseModel>> UpdateUser([FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel { StatusCode = 400, Message = "Invalid request", Data = ModelState });
            }
            return await _userService.UpdateUserAsync(updateUserDto);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<ResponseModel>> DeleteUser(int id)
        {
            return await _userService.DeleteUserAsync(id);
        }

        [HttpPost("upload-image")]
        public async Task<ActionResult<ResponseModel>> UploadUserImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new ResponseModel { StatusCode = 400, Message = "No file uploaded" });
            }

            return await _userService.UploadUserImageAsync(file);
        }
    }
}