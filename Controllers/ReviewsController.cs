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
            // Validasi dokter
            var doctor = await _context.Doctors.FindAsync(dto.DoctorId);
            if (doctor == null)
                return NotFound(new { message = $"Doctor dengan ID {dto.DoctorId} tidak ditemukan." });

            // Validasi user
            var userExists = await _context.Users.AnyAsync(u => u.UserId == dto.UserId);
            if (!userExists)
                return NotFound(new { message = $"User dengan ID {dto.UserId} tidak ditemukan." });

            // Validasi appointment
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == dto.AppointmentId && a.IsActive);
            if (appointment == null)
                return NotFound(new { message = $"Appointment dengan ID {dto.AppointmentId} tidak ditemukan." });

            // Buat review
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
            await _context.SaveChangesAsync();

            return Ok(new { message = "Review berhasil ditambahkan.", review });
        }


        // DELETE: api/reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound(new { message = $"Review dengan ID {id} tidak ditemukan." });
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Review dengan ID {id} berhasil dihapus." });
        }

    }
}
