
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Application.DTOs;
using UserManagementAPI.Application.Interfaces.Services;
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

        [HttpGet("Get")]
        public async Task<ActionResult<UserDto>> GetUser([FromBody] int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("Add")]
        public async Task<ActionResult<int>> AddUser(DcUser userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = await _userService.AddUserAsync(userDto);
            return CreatedAtAction(nameof(GetUser), new { id = userId }, userId);
        }
    }
}
