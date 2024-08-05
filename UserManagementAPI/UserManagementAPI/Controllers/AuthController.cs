using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Application.DTOs;
using UserManagementAPI.Application.Interfaces.Services;
using UserManagementAPI.Domain.Common_Models;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseModel>> Login(LoginDto loginDto)
        {
            return await _authService.LoginAsync(loginDto);
        }
        [HttpPost("activate")]
        public async Task<ActionResult<ResponseModel>> ActivateAccount([FromQuery] int userId)
        {
            return await _authService.ActivateAccountAsync(userId);
        }
        [HttpPost("send-reset-email/{email}")]
        public async Task<ActionResult<ResponseModel>> SendResetPasswordEmail(string email)
        {
            return await _authService.SendResetPasswordEmailAsync(email);
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<ResponseModel>> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            return await _authService.ResetPasswordAsync(resetPasswordDto);
        }
        [HttpPost("change-password")]
        public async Task<ActionResult<ResponseModel>> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            return await _authService.ChangePasswordAsync(changePasswordDto);
        }
    }
}
