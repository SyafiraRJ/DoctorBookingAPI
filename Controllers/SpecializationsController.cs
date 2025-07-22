using DoctorBookingAPI;
using DoctorBookingAPI.DTO.Specializations_DTOs;
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
    public class SpecializationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SpecializationsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Specializations
                .Select(s => new SpecializationDto
                {
                    SpecializationId = s.SpecializationId,
                    Name = s.Name,
                    Description = s.Description,
                    Icon = s.Icon
                })
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var specialization = await _context.Specializations
                .Where(s => s.SpecializationId == id)
                .Select(s => new SpecializationDto
                {
                    SpecializationId = s.SpecializationId,
                    Name = s.Name,
                    Description = s.Description,
                    Icon = s.Icon
                })
                .FirstOrDefaultAsync();

            return specialization == null ? NotFound(new { message = "Specialization not found" }) : Ok(specialization);
        }


        [HttpPost]
        public async Task<IActionResult> Create(SpecializationCreateDto dto)
        {
            var specialization = new Specialization
            {
                Name = dto.Name,
                Icon = dto.Icon,
                Description = dto.Description
            };

            _context.Specializations.Add(specialization);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = specialization.SpecializationId }, specialization);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SpecializationCreateDto dto)
        {
            var specialization = await _context.Specializations.FindAsync(id);
            if (specialization == null) return NotFound(new { message = "Specialization not found" });

            specialization.Name = dto.Name;
            specialization.Icon = dto.Icon;
            specialization.Description = dto.Description;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Specialization updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var specialization = await _context.Specializations.FindAsync(id);
            if (specialization == null) return NotFound(new { message = "Specialization not found" });

            _context.Specializations.Remove(specialization);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Specialization deleted successfully." });
        }

    }

}
