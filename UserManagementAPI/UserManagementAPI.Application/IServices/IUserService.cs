using CollegeManagementAPI.Domain.Common_Models;
using CollegeManagementAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Domain.Entities;

namespace UserManagementAPI.Application.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<int> AddUserAsync(User user);
        Task<ResponseModel> LoginUserAsync(LoginDetails loginDetails);

    }
}
