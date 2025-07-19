using DoctorBookingAPI;
using DoctorBookingAPI.DTO;
using DoctorBookingAPI.DTO.Doctor_DTOs;
using DoctorBookingAPI.DTO.Schedules_DTOs;
using DoctorBookingAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DoctorsController(AppDbContext context) => _context = context;

        // GET: api/doctors
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await _context.Doctors
                .Include(d => d.Specialization)
                .Include(d => d.Provider)
                .Where(d => d.IsActive)
                .Select(d => new DoctorListDto
                {
                    DoctorId = d.DoctorId,
                    FullName = d.FullName,
                    Photo = d.Photo,
                    SpecializationName = d.Specialization.Name,
                    ProviderName = d.Provider.Name,
                    ConsultationFee = d.ConsultationFee ?? 0,
                    Rating = d.Rating ?? 0,
                    ReviewCount = d.ReviewCount ?? 0
                })
                .ToListAsync();

            return Ok(doctors);
        }

        // GET: api/doctors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Specialization)
                .Include(d => d.Provider)
                .Include(d => d.Reviews)
                .Include(d => d.DoctorSchedules)
                .FirstOrDefaultAsync(d => d.DoctorId == id);

            if (doctor == null) return NotFound();

            var dto = new DoctorDetailDto
            {
                DoctorId = doctor.DoctorId,
                FullName = doctor.FullName,
                Photo = doctor.Photo,
                SpecializationName = doctor.Specialization?.Name ?? "",
                ProviderName = doctor.Provider?.Name ?? "",
                ProviderAddress = doctor.Provider?.Address ?? "",
                GoogleMapsLink = doctor.Provider?.GoogleMapsLink ?? "",
                ConsultationFee = doctor.ConsultationFee ?? 0,
                Biography = doctor.Biography ?? "",
                Rating = doctor.Rating ?? 0,
                ReviewCount = doctor.ReviewCount ?? 0,
                Reviews = doctor.Reviews.Select(r => new ReviewDto
                {
                    ReviewId = r.ReviewId,
                    PatientName = r.User?.FullName ?? "Anonymous",
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                }).ToList(),
                Schedules = doctor.DoctorSchedules?.Select(s => new DoctorScheduleDto
                {
                    SchedulesID = s.SchedulesID,
                    DayOfWeek = GetDayName(s.DayOfWeek),
                    StartTime = s.StartTime.ToString(@"hh\:mm"),
                    EndTime = s.EndTime.ToString(@"hh\:mm"),
                    SlotDuration = s.SlotDuration ?? 30,
                    IsActive = s.IsActive
                }).ToList() ?? new List<DoctorScheduleDto>()
            };

            return Ok(dto);
        }

        // POST: api/doctors
        [HttpPost]
        public async Task<IActionResult> Create(DoctorCreateDto model)
        {
            var doctor = new Doctor
            {
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Photo = model.Photo,
                SpecializationId = model.SpecializationId,
                ProviderId = model.ProviderId,
                LicenseNumber = model.LicenseNumber,
                ConsultationFee = model.ConsultationFee,
                Biography = model.Biography,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            var response = new DoctorResponseDto
            {
                DoctorId = doctor.DoctorId,
                FullName = doctor.FullName,
                Email = doctor.Email,
                PhoneNumber = doctor.PhoneNumber,
                Photo = doctor.Photo,
                LicenseNumber = doctor.LicenseNumber,
                ConsultationFee = doctor.ConsultationFee ?? 0,
                Biography = doctor.Biography,
                SpecializationName = (await _context.Specializations.FindAsync(doctor.SpecializationId))?.Name ?? "",
                ProviderName = (await _context.Providers.FindAsync(doctor.ProviderId))?.Name ?? ""
            };

            return Ok(response);
        }

        // PUT: api/doctors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Doctor model)
        {
            if (id != model.DoctorId) return BadRequest();
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/doctors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Doctors.FindAsync(id);
            if (item == null) return NotFound();
            _context.Doctors.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private string GetDayName(byte dayOfWeek)
        {
            return dayOfWeek switch
            {
                1 => "Senin",
                2 => "Selasa",
                3 => "Rabu",
                4 => "Kamis",
                5 => "Jumat",
                6 => "Sabtu",
                7 => "Minggu",
                _ => "Unknown"
            };
        }
    }
}
