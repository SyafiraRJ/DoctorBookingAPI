using DoctorBookingAPI;
using DoctorBookingAPI.DTO;
using DoctorBookingAPI.DTO.Appointment_DTOs;
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
    public class AppointmentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AppointmentsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? status, [FromQuery] DateTime? date)
        {
            var query = _context.Appointments
                .Include(a => a.Doctor).ThenInclude(d => d.Specialization)
                .Include(a => a.Doctor).ThenInclude(d => d.Provider)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(a => a.Status == status);

            if (date.HasValue)
                query = query.Where(a => a.AppointmentDate.Date == date.Value.Date);

            var data = await query
                .Select(a => new AppointmentSummaryDto
                {
                    AppointmentId = a.AppointmentId,
                    DoctorName = a.Doctor.FullName,
                    DoctorPhoto = a.Doctor.Photo,
                    SpecializationName = a.Doctor.Specialization.Name,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.AppointmentTime.ToString(@"hh\:mm"),
                    Status = a.Status,
                    ProviderName = a.Doctor.Provider.Name,
                    QueueNumber = a.QueueNumber,
                    ConsultationFee = a.ConsultationFee ?? 0,
                    PatientNotes = a.PatientNotes
                })
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("history/{userId}")]
        public async Task<IActionResult> GetHistory(int userId)
        {
            var data = await _context.Appointments
                .Where(a => a.UserId == userId && (a.Status == "Completed" || a.Status == "Cancelled"))
                .Include(a => a.Doctor).ThenInclude(d => d.Specialization)
                .Include(a => a.Doctor).ThenInclude(d => d.Provider)
                .Select(a => new AppointmentSummaryDto
                {
                    AppointmentId = a.AppointmentId,
                    DoctorName = a.Doctor.FullName,
                    DoctorPhoto = a.Doctor.Photo,
                    SpecializationName = a.Doctor.Specialization.Name,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.AppointmentTime.ToString(@"hh\:mm"),
                    Status = a.Status,
                    ProviderName = a.Doctor.Provider.Name,
                    QueueNumber = a.QueueNumber,
                    ConsultationFee = a.ConsultationFee ?? 0,
                    PatientNotes = a.PatientNotes
                })
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Doctor).ThenInclude(d => d.Specialization)
                .Include(a => a.Doctor).ThenInclude(d => d.Provider)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);

            if (appointment == null)
                return NotFound();

            var dto = new AppointmentDetailDto
            {
                AppointmentId = appointment.AppointmentId,
                DoctorName = appointment.Doctor.FullName,
                Specialization = appointment.Doctor.Specialization.Name,
                Provider = appointment.Doctor.Provider.Name,
                Photo = appointment.Doctor.Photo,
                AppointmentDate = appointment.AppointmentDate,
                AppointmentTime = appointment.AppointmentTime,
                EndTime = appointment.EndTime,
                Status = appointment.Status,
                PatientNotes = appointment.PatientNotes,
                ConsultationFee = appointment.ConsultationFee
            };

            return Ok(dto);
        }

        [HttpPost("qr-appointment")]
        public async Task<IActionResult> CreateQRAppointment(CreateQRAppointmentDto dto)
        {
            // Find available doctor by specialization
            var availableDoctor = await _context.Doctors
                .Where(d => d.SpecializationId == dto.SpecializationId && d.IsActive)
                .FirstOrDefaultAsync();

            if (availableDoctor == null)
                return BadRequest("Tidak ada dokter tersedia untuk spesialisasi ini.");

            var appointment = new Appointment
            {
                UserId = 0, // Guest appointment
                DoctorId = availableDoctor.DoctorId,
                AppointmentDate = DateTime.Today,
                AppointmentTime = TimeSpan.FromHours(9), // Default 09:00
                EndTime = TimeSpan.FromHours(9.5), // 30 minutes
                Status = "Scheduled",
                Symptoms = dto.Symptoms,
                QueueNumber = GenerateQueueNumber(),
                ConsultationFee = availableDoctor.ConsultationFee,
                PatientNotes = $"QR Appointment - {dto.FullName} ({dto.PhoneNumber})"
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                AppointmentId = appointment.AppointmentId,
                QueueNumber = appointment.QueueNumber,
                DoctorName = availableDoctor.FullName,
                Message = "QR Appointment berhasil dibuat"
            });
        }

        [HttpGet("queue/{appointmentId}")]
        public async Task<IActionResult> GetQueueInfo(int appointmentId)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (appointment == null)
                return NotFound();

            // Count appointments before this one on same date and doctor
            var queuePosition = await _context.Appointments
                .CountAsync(a => a.DoctorId == appointment.DoctorId
                              && a.AppointmentDate == appointment.AppointmentDate
                              && a.AppointmentTime < appointment.AppointmentTime
                              && a.Status == "Scheduled");

            var queueInfo = new QueueInfoDto
            {
                QueueNumber = appointment.QueueNumber ?? "N/A",
                EstimatedWaitTime = queuePosition * 30, // 30 minutes per patient
                CurrentQueue = queuePosition + 1,
                AppointmentDate = appointment.AppointmentDate,
                AppointmentTime = appointment.AppointmentTime.ToString(@"hh\:mm")
            };

            return Ok(queueInfo);
        }

        private string GenerateQueueNumber()
        {
            var today = DateTime.Today;
            var count = _context.Appointments
                .Count(a => a.AppointmentDate == today) + 1;

            return $"A-{count:D3}"; // Format: A-001, A-002, etc.
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppointmentCreateDto dto)
        {
            // Fix DayOfWeek calculation
            var dayOfWeek = (int)dto.AppointmentDate.DayOfWeek;
            if (dayOfWeek == 0) dayOfWeek = 7; // Sunday = 7 instead of 0

            // Cek jika slot sudah dibooking
            var conflict = await _context.Appointments.AnyAsync(a =>
                a.DoctorId == dto.DoctorId &&
                a.AppointmentDate == dto.AppointmentDate &&
                a.AppointmentTime == dto.AppointmentTime &&
                a.IsActive);

            if (conflict)
                return BadRequest("Waktu konsultasi sudah dibooking.");

            // Cari durasi dari jadwal dengan fix DayOfWeek
            var schedule = await _context.DoctorSchedules
                .FirstOrDefaultAsync(s => s.DoctorID == dto.DoctorId && s.DayOfWeek == dayOfWeek);

            if (schedule == null)
                return BadRequest($"Dokter tidak memiliki jadwal di hari {dto.AppointmentDate.DayOfWeek}.");

            // Cek apakah waktu dalam range jadwal dokter
            if (dto.AppointmentTime < schedule.StartTime || dto.AppointmentTime >= schedule.EndTime)
                return BadRequest("Waktu yang dipilih di luar jam praktik dokter.");

            var appointment = new Appointment
            {
                UserId = dto.UserId,
                DoctorId = dto.DoctorId,
                AppointmentDate = dto.AppointmentDate,
                AppointmentTime = dto.AppointmentTime,
                EndTime = dto.AppointmentTime.Add(
                    TimeSpan.FromMinutes((double)(schedule.SlotDuration > 0 ? schedule.SlotDuration : 30))
                ),
                Status = "Scheduled",
                PatientNotes = dto.PatientNotes,
                QueueNumber = GenerateQueueNumber(),
                ConsultationFee = await _context.Doctors
                    .Where(d => d.DoctorId == dto.DoctorId)
                    .Select(d => d.ConsultationFee)
                    .FirstOrDefaultAsync()
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return Ok(appointment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AppointmentUpdateDto dto)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return NotFound("Appointment tidak ditemukan.");

            // Update hanya field yang perlu
            appointment.AppointmentDate = dto.AppointmentDate;
            appointment.AppointmentTime = dto.AppointmentTime;
            appointment.EndTime = dto.AppointmentTime.Add(TimeSpan.FromMinutes(30)); // default 30 menit
            appointment.Status = dto.Status ?? appointment.Status;
            appointment.PatientNotes = dto.PatientNotes;

            _context.Entry(appointment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Kembalikan data ringkas
            var response = new AppointmentSummaryDto
            {
                AppointmentId = appointment.AppointmentId,
                DoctorName = (await _context.Doctors.FindAsync(appointment.DoctorId))?.FullName ?? "",
                DoctorPhoto = (await _context.Doctors.FindAsync(appointment.DoctorId))?.Photo ?? "",
                SpecializationName = (await _context.Doctors
                    .Include(d => d.Specialization)
                    .Where(d => d.DoctorId == appointment.DoctorId)
                    .Select(d => d.Specialization.Name)
                    .FirstOrDefaultAsync()) ?? "",
                AppointmentDate = appointment.AppointmentDate,
                AppointmentTime = appointment.AppointmentTime.ToString(@"hh\:mm"),
                Status = appointment.Status,
                ProviderName = (await _context.Doctors
                    .Include(d => d.Provider)
                    .Where(d => d.DoctorId == appointment.DoctorId)
                    .Select(d => d.Provider.Name)
                    .FirstOrDefaultAsync()) ?? "",
                QueueNumber = appointment.QueueNumber ?? "",
                ConsultationFee = appointment.ConsultationFee ?? 0
            };

            return Ok(response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Appointments.FindAsync(id);
            if (item == null) return NotFound();
            _context.Appointments.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
