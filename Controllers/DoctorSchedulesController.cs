using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoctorBookingAPI;
using DoctorBookingAPI.Model;
using DoctorBookingAPI.DTO.Schedules_DTOs;

namespace DoctorBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorSchedulesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DoctorSchedulesController(AppDbContext context) => _context = context;

        // GET: api/doctorschedules/doctor/1
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetByDoctor(int doctorId)
        {
            var schedules = await _context.DoctorSchedules
                .Where(s => s.DoctorID == doctorId && s.IsActive)
                .ToListAsync();

            var result = schedules.Select(s => new DoctorScheduleDto
            {
                SchedulesID = s.SchedulesID,
                DayOfWeek = GetDayName(s.DayOfWeek),
                StartTime = s.StartTime.ToString(@"hh\:mm"),
                EndTime = s.EndTime.ToString(@"hh\:mm"),
                SlotDuration = s.SlotDuration ?? 30,
                IsActive = s.IsActive
            }).ToList();

            return Ok(result);
        }

        // GET: api/doctorschedules/available?doctorId=1&date=2025-07-20
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableSlots(int doctorId, DateTime date)
        {
            var dayOfWeek = (int)date.DayOfWeek;
            if (dayOfWeek == 0) dayOfWeek = 7; // Minggu = 7

            // Ambil semua jadwal dokter untuk hari itu
            var schedules = await _context.DoctorSchedules
                .Where(s => s.DoctorID == doctorId && s.DayOfWeek == dayOfWeek && s.IsActive)
                .OrderBy(s => s.StartTime)
                .ToListAsync();

            if (!schedules.Any())
                return NotFound("Dokter tidak memiliki jadwal pada hari tersebut.");

            // Ambil semua slot yang sudah dibooking di hari itu
            var bookedSlots = await _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate == date && a.IsActive)
                .Select(a => a.AppointmentTime)
                .ToListAsync();

            var slots = new List<TimeSlotDto>();

            // Loop setiap jadwal (pagi, sore, dll.)
            foreach (var schedule in schedules)
            {
                var current = schedule.StartTime;
                var end = schedule.EndTime;
                var duration = TimeSpan.FromMinutes(schedule.SlotDuration ?? 30);

                while (current + duration <= end)
                {
                    slots.Add(new TimeSlotDto
                    {
                        Time = current.ToString(@"hh\:mm"),
                        IsAvailable = !bookedSlots.Contains(current),
                        Session = GetSessionName(current)
                    });

                    current = current.Add(duration);
                }
            }

            return Ok(slots);
        }


        // POST: api/doctorschedules
        [HttpPost]
        public async Task<IActionResult> Create(DoctorScheduleCreateDto dto)
        {
            var schedule = new DoctorSchedule
            {
                DoctorID = dto.DoctorID,
                DayOfWeek = dto.DayOfWeek,
                StartTime = TimeSpan.Parse(dto.StartTime),
                EndTime = TimeSpan.Parse(dto.EndTime),
                SlotDuration = dto.SlotDuration,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.Now
            };

            _context.DoctorSchedules.Add(schedule);
            await _context.SaveChangesAsync();

            return Ok(schedule);
        }

        // PUT: api/doctorschedules/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DoctorScheduleCreateDto dto)
        {
            var schedule = await _context.DoctorSchedules.FindAsync(id);
            if (schedule == null)
                return NotFound("Schedule not found.");

            schedule.DoctorID = dto.DoctorID;
            schedule.DayOfWeek = dto.DayOfWeek;
            schedule.StartTime = TimeSpan.Parse(dto.StartTime);
            schedule.EndTime = TimeSpan.Parse(dto.EndTime);
            schedule.SlotDuration = dto.SlotDuration;
            schedule.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return Ok(schedule);
        }

        // DELETE: api/doctorschedules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.DoctorSchedules.FindAsync(id);
            if (item == null) return NotFound();
            _context.DoctorSchedules.Remove(item);
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
        private string GetSessionName(TimeSpan time)
        {
            var hour = time.Hours;

            if (hour >= 5 && hour < 12)
                return "Morning";
            if (hour >= 12 && hour < 17)
                return "Afternoon";
            if (hour >= 17 && hour < 21)
                return "Evening";
            if (hour >= 21 && hour < 24)
                return "Night";
            return "Late Night"; // 00:00 - 04:59
        }
    

    }
}
