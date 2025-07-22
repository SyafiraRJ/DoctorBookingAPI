using DoctorBookingAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DoctorBookingAPI
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<DoctorBookingAPI.Model.Appointment> Appointments { get; set; } = default!;
        public DbSet<DoctorBookingAPI.Model.DoctorSchedule> DoctorSchedules { get; set; } = default!;
        public DbSet<Review> Reviews { get; set; }
        public DbSet<DoctorBookingAPI.Model.Blog> Blogs { get; set; } = default!;

    }
}
