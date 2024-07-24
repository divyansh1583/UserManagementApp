using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using UserManagementAPI.Application.Repositories;
using UserManagementAPI.Domain.Common_Models;
using UserManagementAPI.Domain.Entities;
using UserManagementAPI.Infrastructure.Data;

namespace UserManagementAPI.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManagementContext _context;

        public UserRepository(UserManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<DcUser> GetUserByEmailAsync(string email)
        {
            var parameters = new[]
            {
            new SqlParameter("@Email", email),
            };
            var user = await _context.DcUsers.FromSqlRaw(
                "SELECT * FROM DC_Users WHERE CONVERT(VARCHAR(100), DECRYPTBYPASSPHRASE('YourSecretKey', Email)) = @Email",
                parameters
            ).FirstOrDefaultAsync();

            return user;

        }


        public async Task<int> AddUserAsync(User user)
        {
            var parameters = new[]
            {
                new SqlParameter("@FirstName", user.FirstName),
                new SqlParameter("@LastName", user.LastName ?? (object)DBNull.Value),
                new SqlParameter("@MiddleName", user.MiddleName),
                new SqlParameter("@Gender", user.Gender ?? (object)DBNull.Value),
                new SqlParameter("@DateOfJoining", user.DateOfJoining ?? (object)DBNull.Value),
                new SqlParameter("@DateOfBirth", user.DateOfBirth ?? (object)DBNull.Value),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@Phone", user.Phone),
                new SqlParameter("@AlternatePhone", user.AlternatePhone ?? (object)DBNull.Value),
                new SqlParameter("@ImagePath", DBNull.Value), // default value
                new SqlParameter("@CreatedBy", DBNull.Value), // default value
                new SqlParameter("@IsActive", user.IsActive),
                new SqlParameter("@PrimaryAddress", user.Address1 ?? (object)DBNull.Value),
                new SqlParameter("@PrimaryCity", user.City1 ?? (object)DBNull.Value),
                new SqlParameter("@PrimaryState", user.State1 ?? (object)DBNull.Value),
                new SqlParameter("@PrimaryCountry", user.Country1 ?? (object)DBNull.Value),
                new SqlParameter("@PrimaryZipCode", user.ZipCode1 ?? (object)DBNull.Value),
                new SqlParameter("@SecondaryAddress", user.Address2 ?? (object)DBNull.Value),
                new SqlParameter("@SecondaryCity", user.City2 ?? (object)DBNull.Value),
                new SqlParameter("@SecondaryState", user.State2 ?? (object)DBNull.Value),
                new SqlParameter("@SecondaryCountry", user.Country2 ?? (object)DBNull.Value),
                new SqlParameter("@SecondaryZipCode", user.ZipCode2 ?? (object)DBNull.Value),
                new SqlParameter("@Password", user.Password)
            };

            var result = await _context.Database
                .ExecuteSqlRawAsync("EXEC DC_AddUser @FirstName, @LastName, @MiddleName, @Gender, @DateOfJoining, @DateOfBirth, @Email, @Phone, @AlternatePhone, @ImagePath, @CreatedBy, @IsActive, @PrimaryAddress, @PrimaryCity, @PrimaryState, @PrimaryCountry, @PrimaryZipCode, @SecondaryAddress, @SecondaryCity, @SecondaryState, @SecondaryCountry, @SecondaryZipCode, @Password", parameters);

            return result;
        }



        //Login
        public async Task<ResponseModel> LoginAsync(LoginDetails loginDetails)
        {
            var parameters = new[]
            {
            new SqlParameter("@Email", loginDetails.Email),
            new SqlParameter("@Password", loginDetails.Password)
            };

            var user = await _context.DcUsers.FromSqlRaw(
                "SELECT * FROM DC_Users WHERE CONVERT(VARCHAR(100), DECRYPTBYPASSPHRASE('YourSecretKey', Email)) = @Email",
                parameters
            ).FirstOrDefaultAsync();

            if (user != null)
            {
                if (user.Password == loginDetails.Password)
                {
                    return new ResponseModel { StatusCode = 200, Data = new { user = loginDetails, token = "" }, Message = ResponseMessages.UserFound };
                }
                else
                {
                    return new ResponseModel { StatusCode = 401, Data = null, Message = ResponseMessages.InvaildPassword };
                }
            }
            else
            {
                return new ResponseModel { StatusCode = 404, Data = null, Message = ResponseMessages.UserNotFound };
            }
        }
    }
}