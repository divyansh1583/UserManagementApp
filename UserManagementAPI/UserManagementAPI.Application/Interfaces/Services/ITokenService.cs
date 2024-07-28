using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Domain.Entities;

namespace UserManagementAPI.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateToken();
    }
}
