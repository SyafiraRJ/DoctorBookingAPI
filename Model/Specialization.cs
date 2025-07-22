using System.Numerics;

namespace DoctorBookingAPI.Model
{
    public class Specialization
    {
        public int SpecializationId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Doctor>? Doctors { get; set; } // Navigasi opsional
    }
}
