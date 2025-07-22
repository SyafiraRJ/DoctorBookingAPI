using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoctorBookingAPI;
using DoctorBookingAPI.Model;
using DoctorBookingAPI.DTO.Blog_DTOs;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace DoctorBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BlogsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Blogs
                .Where(b => b.IsActive)
                .Select(b => new BlogDto
                {
                    BlogId = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    ImageUrl = b.ImageUrl
                })
                .ToListAsync();
            return Ok(data);
        }

        // GET: api/Blogs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlog(int id)
        {
            var blog = await _context.Blogs
                .Where(b => b.Id == id && b.IsActive)
                .Select(b => new BlogDto
                {
                    BlogId = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    ImageUrl = b.ImageUrl
                })
            .FirstOrDefaultAsync();

            if (blog == null)
                return NotFound(new { message = "Blog not found" });

            return Ok(blog);
        }

        // PUT: api/Blogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlog(int id, BlogCreateDto dto)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null) return NotFound(new { message = "Blog not found" });


            blog.Title = dto.Title;
            blog.Description = dto.Description;
            blog.ImageUrl = dto.ImageUrl;
            //blog.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Blog id updated successfully.", blog });
        }

        // POST: api/Blogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostBlog( BlogCreateDto dto)
        {
            var blog = new Blog
            {
                Title = dto.Title,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Create Blog successful", data = blog });
        }

        // DELETE: api/Blogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound(new {message = "Blog not found."});
            }

            if (!blog.IsActive)
                return BadRequest(new { message = $"Blog {id} is already deactivated." });

            blog.IsActive = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Blog {id} has been deactivated." });
        }
    }
}
