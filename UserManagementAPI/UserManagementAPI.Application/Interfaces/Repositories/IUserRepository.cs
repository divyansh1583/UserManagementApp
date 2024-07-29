
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Domain.Common_Models;
using UserManagementAPI.Domain.Entities;



namespace UserManagementAPI.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<DcUser> GetByIdAsync(int id);
        Task<DcUser> GetByEmailAsync(string email);
        Task<List<DcUser>> GetAllAsync();
        Task<int> AddAsync(DcUser user);
        Task<bool> UpdateAsync(DcUser user);
        Task<bool> DeleteAsync(int id); 
    }
}
