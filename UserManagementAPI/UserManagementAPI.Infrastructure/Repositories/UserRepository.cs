
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Domain.Common_Models;
using UserManagementAPI.Domain.Entities;

using BCrypt.Net;
using UserManagementAPI.Application.Interfaces.Repositories;
using UserManagementAPI.Domain;
using UserManagementAPI.Application.DTOs;
using UserManagementAPI.Application.Interfaces.Services;

namespace UserManagementAPI.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        private readonly IEncryptionService _encryptionService;

        public UserRepository(UserDbContext context, IEncryptionService encryptionService)
        {
            _context = context;
            _encryptionService = encryptionService;
        }

        public async Task<DcUser> GetByIdAsync(int id)
        {
            var user = await _context.DcUsers
                .Include(u => u.DcUserAddresses)
                .FirstOrDefaultAsync(u => u.UserId == id);
            return user;
        }
        public async Task<DcUser> GetByEmailAsync(string email)
        {
            var users = await GetAllAsync();
            var user = users.FirstOrDefault(u => _encryptionService.Decrypt(u.Email) == email);
            return user;
        }

        public async Task<List<DcUser>> GetAllAsync()
        {
            var users = await _context.DcUsers
                .Include(u => u.DcUserAddresses)
                .ToListAsync();
            return users;
        }

        public async Task<int> AddAsync(DcUser user)
        {
            await _context.DcUsers.AddAsync(user);
            var users=await _context.SaveChangesAsync();

            return users;
        }

        public async Task<bool> UpdateAsync(DcUser user)
        {
            _context.DcUsers.Update(user);
            var users = await _context.SaveChangesAsync();
            return users > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var parameters = new SqlParameter[]
            {
             new SqlParameter("@UserId", id)
            };

            var result = await _context.Database.ExecuteSqlRawAsync("EXEC DC_DeleteUser @UserId", parameters);

            return result < 0;
        }

    }
}