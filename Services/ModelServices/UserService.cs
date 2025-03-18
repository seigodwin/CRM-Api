using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CRMApi.DbContexts;
using CRMApi.Domain.DTOs;
using CRMApi.Domain.Models;
using CRMApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CRMApi.Services.Services
{
    public class UserService(CRMApiDbContext context, IConfiguration config) : IUserService
    {
        private readonly CRMApiDbContext _context = context;
        private readonly IConfiguration _config = config;
        

        public async Task<ServiceResponse<string>> Login(LoginDTO loginDTO)
        {
            var response = new ServiceResponse<string>();

            if (loginDTO is null)
            {
                response.Message = "Login cridentials is null";
                response.Success = false;
                return response;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

            if (user is null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password))
            {
                response.Message = "Invalid Cridentials";
                response.Success = false;
                return response;
            }

            response.Data = GenerateJwtToken(user);
            response.Message = "Login Successful";
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<bool>> Register(UserDTO userDTO)
        {
            var response = new ServiceResponse<bool>();

            if (userDTO is null)
            {
                response.Message = "Sign Up cridentials is null";
                response.Success = false;
                return response;
            }

            var user = new User
            {
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                Password = userDTO.Password
            };

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                response.Message = "User created Successfully";
                response.Success = true;
            }

            catch(DbUpdateException dbEx)
            {
                response.Message = $"A database error occured while adding new User: {dbEx.Message}";
                response.Success = false;
            }

            return response;
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
