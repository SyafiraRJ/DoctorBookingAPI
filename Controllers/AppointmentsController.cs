using DoctorBookingAPI;
using DoctorBookingAPI.DTO;
using DoctorBookingAPI.DTO.Appointment_DTOs;
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

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var appointments = await _context.Appointments
                .Where(a => a.UserId == userId && a.IsActive)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.Specialization)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.Provider)
                .Select(a => new AppointmentDetailDto
                {
                    AppointmentId = a.AppointmentId,
                    DoctorName = a.Doctor.FullName,
                    Specialization = a.Doctor.Specialization.Name,
                    Provider = a.Doctor.Provider.Name,
                    Photo = a.Doctor.Photo,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.AppointmentTime,
                    EndTime = a.EndTime,
                    Status = a.Status,
                    PatientNotes = a.PatientNotes,
                    ConsultationFee = a.ConsultationFee
                })
                .ToListAsync();

            return Ok(appointments);
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

        [HttpPost]
        public async Task<IActionResult> Create(AppointmentCreateDto dto)
        {
            var userExists = await _context.Users.AnyAsync(u => u.UserId == dto.UserId);
            if (!userExists)
                return BadRequest($"User dengan ID {dto.UserId} tidak ditemukan.");

            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.DoctorId == dto.DoctorId);
            if (doctor == null)
                return BadRequest($"Dokter dengan ID {dto.DoctorId} tidak ditemukan.");

            // Fix DayOfWeek calculation
            var dayOfWeek = (int)dto.AppointmentDate.DayOfWeek;
            if (dayOfWeek == 0) dayOfWeek = 7; // Sunday = 7 instead of 0

            // Cek jadwal dokter
            var schedule = await _context.DoctorSchedules
                .FirstOrDefaultAsync(s => s.DoctorID == dto.DoctorId && s.DayOfWeek == dayOfWeek && s.IsActive);

            if (schedule == null)
                return BadRequest($"Dokter tidak memiliki jadwal di hari {dto.AppointmentDate.DayOfWeek}.");

            // Cek apakah waktu dalam range jadwal dokter
            if (dto.AppointmentTime < schedule.StartTime || dto.AppointmentTime >= schedule.EndTime)
                return BadRequest($"Waktu yang dipilih ({dto.AppointmentTime:hh\\:mm}) di luar jam praktik dokter ({schedule.StartTime:hh\\:mm} - {schedule.EndTime:hh\\:mm}).");

            // Cek jika slot sudah dibooking
            var conflict = await _context.Appointments.AnyAsync(a =>
                a.DoctorId == dto.DoctorId &&
                a.AppointmentDate == dto.AppointmentDate &&
                a.AppointmentTime == dto.AppointmentTime &&
                a.IsActive &&
                a.Status != "Cancelled");

            if (conflict)
                return BadRequest("Waktu konsultasi sudah dibooking.");

            var slotDuration = schedule.SlotDuration > 0 ? schedule.SlotDuration : 30;
            var queueNumber = await GenerateQueueNumber(dto.DoctorId, dto.AppointmentDate);

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
                Symptoms = dto.Symptoms,
                QueueNumber = queueNumber,
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
            appointment.Symptoms = dto.Symptoms;
            appointment.PatientNotes = dto.PatientNotes;

            //_context.Entry(appointment).State = EntityState.Modified;

            // Update EndTime sesuai SlotDuration dokter
            var schedule = await _context.DoctorSchedules
                .FirstOrDefaultAsync(s => s.DoctorID == appointment.DoctorId &&
                                          s.DayOfWeek == (int)dto.AppointmentDate.DayOfWeek);

            if (schedule != null)
            {
                appointment.EndTime = dto.AppointmentTime.Add(
                    TimeSpan.FromMinutes((double)(schedule.SlotDuration > 0 ? schedule.SlotDuration : 30))
                );
            }

            appointment.UpdatedAt = DateTime.Now;

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
        public async Task<IActionResult> Cancel(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return NotFound();

            // Soft delete: ubah status dan aktifasi
            appointment.Status = "Cancelled";
            appointment.IsActive = false;
            appointment.UpdatedAt = DateTime.Now; // kalau punya UpdatedAt

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Appointment berhasil dibatalkan." });
        }

        [HttpPost("qr-appointment")]
        public async Task<IActionResult> CreateFromQR([FromBody] CreateQRAppointmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest($"Validation Error: {errors}");
            }

            try
            {
                // Validasi manual field penting
                if (dto.UserId <= 0)
                    return BadRequest("UserId tidak boleh kosong atau 0.");
                if (string.IsNullOrWhiteSpace(dto.QRData))
                    return BadRequest("QRData tidak boleh kosong.");
                if (string.IsNullOrWhiteSpace(dto.Symptoms))
                    return BadRequest("Symptoms harus diisi.");

                // Parsing QRData
                var qrParts = ParseQRData(dto.QRData);
                if (qrParts == null)
                    return BadRequest("Format QRData tidak valid. Gunakan DOCTOR-{ID}|{YYYY-MM-DD}|{HH:MM}");

                var (doctorId, appointmentDate, appointmentTime) = qrParts.Value;

                // Validasi user
                var userExists = await _context.Users.AnyAsync(u => u.UserId == dto.UserId);
                if (!userExists)
                    return BadRequest($"User dengan ID {dto.UserId} tidak ditemukan.");

                // Validasi dokter
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.DoctorId == doctorId);
                if (doctor == null)
                    return BadRequest($"Dokter dengan ID {doctorId} tidak ditemukan.");

                // Cek jadwal dokter
                var dayOfWeek = (int)appointmentDate.DayOfWeek;
                if (dayOfWeek == 0) dayOfWeek = 7;
                var schedule = await _context.DoctorSchedules
                    .FirstOrDefaultAsync(s => s.DoctorID == doctorId && s.DayOfWeek == dayOfWeek && s.IsActive);

                if (schedule == null)
                    return BadRequest($"Dokter tidak memiliki jadwal di hari {appointmentDate.DayOfWeek}.");

                // Cek waktu valid
                if (appointmentTime < schedule.StartTime || appointmentTime >= schedule.EndTime)
                    return BadRequest($"Waktu {appointmentTime:hh\\:mm} di luar jam praktik ({schedule.StartTime:hh\\:mm} - {schedule.EndTime:hh\\:mm}).");

                // Cek slot
                var conflict = await _context.Appointments.AnyAsync(a =>
                    a.DoctorId == doctorId &&
                    a.AppointmentDate == appointmentDate &&
                    a.AppointmentTime == appointmentTime &&
                    a.IsActive &&
                    a.Status != "Cancelled");

                if (conflict)
                    return BadRequest("Waktu konsultasi sudah dibooking.");

                // Generate queue
                var slotDuration = schedule.SlotDuration > 0 ? schedule.SlotDuration : 30;
                var queueNumber = await GenerateQueueNumber(doctorId, appointmentDate);

                // Create appointment - FIXED: Added missing required fields
                var appointment = new Appointment
                {
                    UserId = dto.UserId,
                    DoctorId = doctorId,
                    AppointmentDate = appointmentDate,
                    AppointmentTime = appointmentTime,
                    EndTime = appointmentTime.Add(TimeSpan.FromMinutes((double)slotDuration)),
                    Status = "Scheduled",
                    PatientNotes = dto.PatientNotes ?? "Booking via QR",
                    Symptoms = dto.Symptoms,
                    QueueNumber = queueNumber,
                    ConsultationFee = doctor.ConsultationFee,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsActive = true
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                return Ok(appointment); // Changed from GetAppointmentSummary to match working method
            }
            catch (Exception ex)
            {
                // Log the full exception details
                Console.WriteLine($"Exception: {ex}");
                return StatusCode(500, $"Terjadi kesalahan: {ex.Message}\nInner: {ex.InnerException?.Message}\nStackTrace: {ex.StackTrace}");
            }
        }

        [HttpGet("queue/{appointmentId}")]
        public async Task<IActionResult> GetQueueInfo(int appointmentId)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (appointment == null)
                return NotFound("Appointment tidak ditemukan.");

            // Count appointments before this one on same date and doctor (by appointment time)
            var appointmentsBeforeMe = await _context.Appointments
                .Where(a => a.DoctorId == appointment.DoctorId
                          && a.AppointmentDate == appointment.AppointmentDate
                          && a.AppointmentTime < appointment.AppointmentTime
                          && (a.Status == "Scheduled" || a.Status == "In Progress")
                          && a.IsActive)
                .OrderBy(a => a.AppointmentTime)
                .ToListAsync();

            var currentAppointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.DoctorId == appointment.DoctorId
                                       && a.AppointmentDate == appointment.AppointmentDate
                                       && a.Status == "In Progress"
                                       && a.IsActive);

            var queueInfo = new QueueInfoDto
            {
                QueueNumber = appointment.QueueNumber ?? "N/A",
                EstimatedWaitTime = appointmentsBeforeMe.Count * 30, // 30 minutes per patient
                CurrentQueue = appointmentsBeforeMe.Count + 1,
                TotalQueue = await _context.Appointments
                    .CountAsync(a => a.DoctorId == appointment.DoctorId
                              && a.AppointmentDate == appointment.AppointmentDate
                              && (a.Status == "Scheduled" || a.Status == "In Progress")
                              && a.IsActive),
                AppointmentDate = appointment.AppointmentDate,
                AppointmentTime = appointment.AppointmentTime.ToString(@"hh\:mm"),
                Status = appointment.Status,
                CurrentlyServing = currentAppointment?.QueueNumber ?? "N/A"
            };

            return Ok(queueInfo);
        }

        [HttpPost("next-queue/{doctorId}")]
        public async Task<IActionResult> NextQueue(int doctorId)
        {
            var today = DateTime.Today;
            var nextAppointment = await _context.Appointments
                .Where(a => a.DoctorId == doctorId
                          && a.AppointmentDate == today
                          && a.Status == "Scheduled"
                          && a.IsActive)
                .OrderBy(a => a.AppointmentTime)
                .FirstOrDefaultAsync();

            if (nextAppointment == null)
                return NotFound("Tidak ada appointment berikutnya.");

            // Set current appointment to completed (if any)
            var currentAppointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.DoctorId == doctorId
                                       && a.AppointmentDate == today
                                       && a.Status == "In Progress");

            if (currentAppointment != null)
            {
                currentAppointment.Status = "Completed";
                currentAppointment.UpdatedAt = DateTime.Now;
            }

            // Set next appointment to in progress
            nextAppointment.Status = "In Progress";
            nextAppointment.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Queue berhasil dimajukan.",
                currentQueue = nextAppointment.QueueNumber,
                appointmentId = nextAppointment.AppointmentId
            });
        }

        private async Task<string> GenerateQueueNumber(int doctorId, DateTime appointmentDate)
        {
            string prefix = $"D{doctorId}-{appointmentDate:ddMM}";

            var lastQueueNumber = await _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Date == appointmentDate.Date)
                .OrderByDescending(a => a.AppointmentId)
                .Select(a => a.QueueNumber)
                .FirstOrDefaultAsync();

            int sequentialNumber = 1;
            if (!string.IsNullOrEmpty(lastQueueNumber))
            {
                string[] parts = lastQueueNumber.Split('-');
                string lastNumberStr = parts.Length > 2 ? parts[2] : null;

                if (!string.IsNullOrEmpty(lastNumberStr) && int.TryParse(lastNumberStr, out int lastNumber))
                {
                    sequentialNumber = lastNumber + 1;
                }
            }

            return $"{prefix}-{sequentialNumber}";
        }


        private (int doctorId, DateTime appointmentDate, TimeSpan appointmentTime)? ParseQRData(string qrData)
        {
            try
            {
                // Expected format: DOCTOR-{ID}|{YYYY-MM-DD}|{HH:MM}
                // Example: DOCTOR-1|2025-07-21|14:30

                if (string.IsNullOrWhiteSpace(qrData))
                    return null;

                var parts = qrData.Split('|');
                if (parts.Length != 3)
                    return null;

                // Parse doctor ID
                var doctorPart = parts[0];
                if (!doctorPart.StartsWith("DOCTOR-"))
                    return null;

                var doctorIdStr = doctorPart.Substring("DOCTOR-".Length);
                if (!int.TryParse(doctorIdStr, out int doctorId))
                    return null;

                // Parse date
                if (!DateTime.TryParseExact(parts[1], "yyyy-MM-dd", null, DateTimeStyles.None, out DateTime appointmentDate))
                    return null;

                // Parse time
                if (!TimeSpan.TryParseExact(parts[2], @"hh\:mm", null, out TimeSpan appointmentTime))
                    return null;

                return (doctorId, appointmentDate, appointmentTime);
            }
            catch
            {
                return null;
            }
        }
        private async Task<(bool IsValid, string? ErrorMessage, DoctorSchedule? Schedule)> ValidateAppointmentSlot(
            int doctorId, DateTime appointmentDate, TimeSpan appointmentTime)
        {
            // Cek jadwal dokter
            var dayOfWeek = (int)appointmentDate.DayOfWeek;
            if (dayOfWeek == 0) dayOfWeek = 7; // Minggu = 7

            var schedule = await _context.DoctorSchedules
                .FirstOrDefaultAsync(s => s.DoctorID == doctorId && s.DayOfWeek == dayOfWeek && s.IsActive);

            if (schedule == null)
                return (false, $"Dokter tidak memiliki jadwal di hari {appointmentDate:dddd}.", null);

            // Cek apakah waktu ada dalam rentang jadwal
            if (appointmentTime < schedule.StartTime || appointmentTime >= schedule.EndTime)
                return (false, $"Waktu {appointmentTime:hh\\:mm} di luar jam praktik dokter ({schedule.StartTime:hh\\:mm} - {schedule.EndTime:hh\\:mm}).", schedule);

            // Cek slot sudah dibooking?
            var isBooked = await _context.Appointments.AnyAsync(a =>
                a.DoctorId == doctorId &&
                a.AppointmentDate == appointmentDate &&
                a.AppointmentTime == appointmentTime &&
                a.IsActive &&
                a.Status != "Cancelled");

            if (isBooked)
                return (false, "Slot ini sudah dibooking.", schedule);

            return (true, null, schedule);
        }

        private async Task<AppointmentSummaryDto> GetAppointmentSummary(int appointmentId)
        {
            return await _context.Appointments
                .Where(a => a.AppointmentId == appointmentId)
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
                .FirstOrDefaultAsync();
        }
    }

}
