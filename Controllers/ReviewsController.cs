using DoctorBookingAPI;
using DoctorBookingAPI.DTO;
using DoctorBookingAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ReviewsController(AppDbContext context) => _context = context;

        // GET: api/reviews/doctor/1
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetByDoctor(int doctorId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.DoctorId == doctorId)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewDto
                {
                    ReviewId = r.ReviewId,
                    PatientName = r.User != null ? r.User.FullName : "Anonymous",
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();

            return Ok(reviews);
        }

        // POST: api/reviews
        [HttpPost]
        public async Task<IActionResult> Create(ReviewCreateDto dto)
        {
            var review = new Review
            {
                DoctorId = dto.DoctorId,
                UserId = dto.UserId,
                AppointmentId = dto.AppointmentId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);

            // Update rating dokter
            var doctor = await _context.Doctors.FindAsync(dto.DoctorId);
            if (doctor != null)
            {
                doctor.ReviewCount = (doctor.ReviewCount ?? 0) + 1;
                doctor.Rating = await _context.Reviews
                    .Where(r => r.DoctorId == dto.DoctorId)
                    .AverageAsync(r => r.Rating);
            }

            await _context.SaveChangesAsync();
            return Ok(review);
        }

        // DELETE: api/reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return NotFound();

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
