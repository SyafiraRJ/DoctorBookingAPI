using System.Numerics;

namespace DoctorBookingAPI.Model
{
    public class Provider
    {
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string? GoogleMapsLink { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }

        public ICollection<Doctor>? Doctors { get; set; }
    }
}
