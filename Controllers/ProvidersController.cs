using DoctorBookingAPI;
using DoctorBookingAPI.DTO.Provider_DTOs;
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
    public class ProvidersController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProvidersController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var providers = await _context.Providers
                .Where(p => p.IsActive)
                .Select(p => new ProviderDto
                {
                    ProviderId = p.ProviderId,
                    Name = p.Name,
                    Address = p.Address,
                    City = p.City,
                    GoogleMapsLink = p.GoogleMapsLink
                })
                .ToListAsync();

            return Ok(providers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var provider = await _context.Providers
                .Where(p => p.ProviderId == id && p.IsActive)
                .Select(p => new ProviderDto
                {
                    ProviderId = p.ProviderId,
                    Name = p.Name,
                    Address = p.Address,
                    City = p.City,
                    GoogleMapsLink = p.GoogleMapsLink
                })
                .FirstOrDefaultAsync();

            return provider == null
                ? NotFound(new { message = $"Provider dengan ID {id} tidak ditemukan." })
                : Ok(provider);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProviderCreateDto dto)
        {
            var provider = new Provider
            {
                Name = dto.Name,
                Address = dto.Address,
                City = dto.City,
                GoogleMapsLink = dto.GoogleMapsLink,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Providers.Add(provider);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Provider berhasil ditambahkan.", id = provider.ProviderId });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProviderCreateDto dto)
        {
            var provider = await _context.Providers.FindAsync(id);
            if (provider == null)
                return NotFound(new { message = $"Provider dengan ID {id} tidak ditemukan." });

            provider.Name = dto.Name;
            provider.Address = dto.Address;
            provider.City = dto.City;
            provider.GoogleMapsLink = dto.GoogleMapsLink;
            provider.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Provider berhasil diupdate.", id = provider.ProviderId });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var provider = await _context.Providers.FindAsync(id);
            if (provider == null)
                return NotFound(new { message = $"Provider dengan ID {id} tidak ditemukan." });

            if (!provider.IsActive)
                return BadRequest(new { message = $"Provider dengan ID {id} sudah nonaktif." });

            provider.IsActive = false;
            provider.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = $"Provider dengan ID {id} telah dinonaktifkan." });
        }


    }

}
