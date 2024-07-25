
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
        Task<int> AddAsync(DcUser user);
    }
}
