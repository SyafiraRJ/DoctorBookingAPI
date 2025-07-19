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

        // POST: api/doctorschedules
        [HttpPost]
        public async Task<IActionResult> Create(DoctorSchedule model)
        {
            _context.DoctorSchedules.Add(model);
            await _context.SaveChangesAsync();
            return Ok(model);
        }

        // PUT: api/doctorschedules/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DoctorSchedule model)
        {
            if (id != model.SchedulesID) return BadRequest();
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
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

        // GET: api/doctorschedules/available?doctorId=1&date=2025-07-20
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableSlots(int doctorId, DateTime date)
        {
            var dayOfWeek = (int)date.DayOfWeek;
            if (dayOfWeek == 0) dayOfWeek = 7; // Minggu = 7

            var schedule = await _context.DoctorSchedules
                .FirstOrDefaultAsync(s => s.DoctorID == doctorId && s.DayOfWeek == dayOfWeek && s.IsActive);

            if (schedule == null)
                return NotFound("Dokter tidak memiliki jadwal pada hari tersebut.");

            var bookedSlots = await _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate == date && a.IsActive)
                .Select(a => a.AppointmentTime)
                .ToListAsync();

            var slots = new List<TimeSlotDto>();
            var current = schedule.StartTime;
            var end = schedule.EndTime;
            var duration = TimeSpan.FromMinutes(schedule.SlotDuration ?? 30);

            while (current + duration <= end)
            {
                slots.Add(new TimeSlotDto
                {
                    Time = current.ToString(@"hh\:mm"),
                    IsAvailable = !bookedSlots.Contains(current),
                    Session = current.Hours < 12 ? "Morning" : "Afternoon"
                });

                current = current.Add(duration);
            }

            return Ok(slots);
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
