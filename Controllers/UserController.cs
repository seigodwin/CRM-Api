using CRMApi.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using CRMApi.Domain.Models;
using System;
using CRMApi.Domain.DTOs; 

namespace CRMApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class  UserController : ControllerBase
    {
        private readonly CRMApiDbContext _context;
        private readonly IConfiguration _config;

        public UserController(CRMApiDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        
        [HttpPost("signup")]
        public IActionResult Register([FromBody] UserDTO userDTO) 
        {
            if (userDTO is null)
            {
                return BadRequest("User model is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_context.Users.Any(u => u.Email == userDTO.Email))
            {
                return BadRequest("User already exists!");
            }

            
            var user = new User
            {
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("User registered successfully!");
        }

        
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            if (loginDTO is null)
            {
                return BadRequest("User model is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbUser = _context.Users.FirstOrDefault(u => u.Email == loginDTO.Email);

            if (dbUser == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, dbUser.Password))
            {
                return Unauthorized("Invalid credentials!");
            }

            var token = GenerateJwtToken(dbUser);
            return Ok(new { Token = token }); 
        }

        
        [Authorize]
        [HttpGet("secure-data")]
        public IActionResult SecureData()
        {
            return Ok("This is a protected API.");  
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