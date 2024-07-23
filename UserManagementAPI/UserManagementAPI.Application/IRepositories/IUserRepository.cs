using CollegeManagementAPI.Domain.Common_Models;
using CollegeManagementAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Domain.Entities;

namespace UserManagementAPI.Application.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetUserByEmailAsync(string email);
        Task<int> AddUserAsync(User user);
        Task<ResponseModel> LoginAsync(LoginDetails loginDetails);
    }
}
