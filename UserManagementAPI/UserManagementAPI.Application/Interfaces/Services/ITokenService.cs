using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Application.DTOs;
using UserManagementAPI.Domain.Entities;

namespace UserManagementAPI.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateToken(UserDto userDto);
        bool ValidateToken(string token);
    }
}
