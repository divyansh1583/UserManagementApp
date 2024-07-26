
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Domain.Common_Models;
using UserManagementAPI.Domain.Entities;

using BCrypt.Net;
using UserManagementAPI.Application.Interfaces.Repositories;
using UserManagementAPI.Domain;

namespace UserManagementAPI.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;

        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<DcUser> GetByIdAsync(int id)
        {
            var user = await _context.DcUsers
                .Include(u => u.DcUserAddresses)
                .FirstOrDefaultAsync(u => u.UserId == id);
            return user;
        }

        public async Task<int> AddAsync(DcUser user)
        {
            await _context.DcUsers.AddAsync(user);
            await _context.DcUserAddresses.AddRangeAsync(user.DcUserAddresses);
            var users=await _context.SaveChangesAsync();

            return users;
        }

        public async Task<List<DcUser>> GetAllAsync()
        {
            var users = await _context.DcUsers.ToListAsync();
            return users;
        }

    }
}