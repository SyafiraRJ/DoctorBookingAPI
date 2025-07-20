using DoctorBookingAPI;
using DoctorBookingAPI.DTO;
using DoctorBookingAPI.DTO.User_DTOs;
using DoctorBookingAPI.Model;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DoctorBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("Email already exists.");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registration successful", userId = user.UserId });
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid email or password.");

            // Belum pakai JWT, jadi kirim manual user ID
            return Ok(new
            {
                message = "Login successful",
                userId = user.UserId,
                name = user.FullName
            });
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users
                .Where(u => u.IsActive  )
                .Select(u => new UserDTO
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Gender = u.Gender,
                    DateOfBirth = u.DateOfBirth,
                    Address = u.Address
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _context.Users
                .Where(u => u.UserId == id)
                .Select(u => new UserDTO
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Gender = u.Gender,
                    DateOfBirth = u.DateOfBirth,
                    Address = u.Address,
                    IsActive = u.IsActive
                })
                .FirstOrDefaultAsync();

            if (user == null) return NotFound();
            return Ok(user);
        }


        // PUT: api/users/{id}/profile
        [HttpPut("{id}/profile")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] UserProfileEditDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

                user.Gender = dto.Gender;
                user.DateOfBirth = dto.DateOfBirth;
                user.Address = dto.Address;
                user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Profile updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound(new { message = "User not found." });

            if (!user.IsActive)
                return BadRequest(new { message = $"User {id} is already deactivated." });

            user.IsActive = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"User {id} has been deactivated." });
        }

    }
}
