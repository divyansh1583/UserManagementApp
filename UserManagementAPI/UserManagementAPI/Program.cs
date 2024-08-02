
using CollegeManagementAPI.Infrastructure.Implementation.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserManagementAPI.Application.Interfaces.Repositories;
using UserManagementAPI.Application.Interfaces.Services;
using UserManagementAPI.Domain;
using UserManagementAPI.Infrastructure.Data;
using UserManagementAPI.Infrastructure.Data.Configurations;
using UserManagementAPI.Infrastructure.Repositories;
using UserManagementAPI.Infrastructure.Services;

namespace UserManagementAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var environment = builder.Environment;
            var machineName = Environment.MachineName;
            //"SMARTDATA-497"
            var connectionString = machineName == "LAPTOP-SQTG162V"
                ? builder.Configuration.GetConnectionString("LocalConnection")
                : builder.Configuration.GetConnectionString("DefaultConnection");
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddControllers().AddNewtonsoftJson();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // Add DbContext
            builder.Services.AddDbContext<UserDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });
            // Add repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // Add services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IEncryptionService, EncryptionService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<ITokenService, TokenService>();

            builder.Services.AddAutoMapper(cfg =>
            {
                var serviceProvider = builder.Services.BuildServiceProvider();
                var encryptionService = serviceProvider.GetRequiredService<IEncryptionService>();
                cfg.AddProfile(new MappingProfile(encryptionService));
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseAuthorization();

            app.UseStaticFiles();
            app.MapControllers();

            app.Run();
        }
    }
}
