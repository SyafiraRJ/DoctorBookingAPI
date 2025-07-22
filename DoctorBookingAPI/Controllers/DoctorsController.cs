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
using System.Globalization;
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
                .Include(d => d.Reviews)
                .Where(d => d.IsActive)
                .Select(d => new DoctorListDto
                {
                    DoctorId = d.DoctorId,
                    FullName = d.FullName,
                    Photo = d.Photo,
                    SpecializationName = d.Specialization.Name,
                    ConsultationFee = d.ConsultationFee ?? 0,
                    Rating = d.Reviews.Any()
                        ? d.Reviews.Average(r => r.Rating).ToString("0.0", CultureInfo.InvariantCulture)
                        : "-",
                })
                .ToListAsync();

            return Ok(doctors);
        }

        [HttpGet("by-specialization/{specializationId}")]
        public async Task<IActionResult> GetDoctorsBySpecialization(int specializationId)
        {
            var doctors = await _context.Doctors
                .Include(d => d.Specialization)
                .Include(d => d.Reviews)
                .Where(d => d.SpecializationId == specializationId && d.IsActive)
                .Select(d => new DoctorListDto
                {
                    DoctorId = d.DoctorId,
                    FullName = d.FullName,
                    Photo = d.Photo,
                    SpecializationName = d.Specialization.Name,
                    ConsultationFee = d.ConsultationFee ?? 0,
                    Rating = d.Reviews.Any() 
                        ? d.Reviews.Average(r => r.Rating).ToString("0.0", CultureInfo.InvariantCulture) 
                        : "-",
                })
                .ToListAsync();

            if (!doctors.Any())
                return NotFound(new { message = "No doctors found for this specialization." });

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

            if (doctor == null)
                return NotFound(new { message = "Doctor tidak ditemukan." });

            if(!doctor.IsActive)
                return BadRequest(new { message = "Doctor ini sudah dinonaktifkan." });

            var dto = new DoctorDetailDto
            {
                DoctorId = doctor.DoctorId,
                FullName = doctor.FullName,
                Email = doctor.Email,
                PhoneNumber = doctor.PhoneNumber,
                Photo = doctor.Photo,
                SpecializationName = doctor.Specialization?.Name ?? "",
                ProviderName = doctor.Provider?.Name ?? "",
                ProviderAddress = doctor.Provider?.Address ?? "",
                LicenseNumber = doctor.LicenseNumber,
                ConsultationFee = doctor.ConsultationFee ?? 0,
                Biography = doctor.Biography ?? "",
                //GoogleMapsLink = doctor.Provider?.GoogleMapsLink ?? "",
                Longitude = doctor.Provider?.Longitude ?? 0,
                Latitude = doctor.Provider?.Latitude ?? 0,
                Rating = doctor.Reviews.Any() 
                    ? doctor.Reviews.Average(r => r.Rating).ToString("0.0", CultureInfo.InvariantCulture) 
                    : "-",
                ReviewCount = doctor.Reviews.Count,
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
        public async Task<IActionResult> Update(int id, [FromBody] DoctorCreateDto dto)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
                return NotFound("Doctor tidak ditemukan.");

            // Update semua field dari DTO
            doctor.FullName = dto.FullName;
            doctor.Email = dto.Email;
            doctor.PhoneNumber = dto.PhoneNumber;
            doctor.Photo = dto.Photo;
            doctor.SpecializationId = dto.SpecializationId;
            doctor.ProviderId = dto.ProviderId;
            doctor.LicenseNumber = dto.LicenseNumber;
            doctor.ConsultationFee = dto.ConsultationFee;
            doctor.Biography = dto.Biography;
            doctor.IsActive = dto.IsActive;

            _context.Entry(doctor).State = EntityState.Modified;
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

            return Ok(new { message = "Doctor berhasil diupdate", data = response });
        }

        // DELETE: api/doctors/5
        // DELETE: api/doctors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
                return NotFound(new { message = $"Doctor dengan ID {id} tidak ditemukan." });

            if (!doctor.IsActive)
                return BadRequest(new { message = $"Doctor dengan ID {id} sudah nonaktif." });

            doctor.IsActive = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Doctor dengan ID {id} telah dinonaktifkan." });
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
