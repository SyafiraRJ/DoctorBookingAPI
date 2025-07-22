    namespace DoctorBookingAPI.Model
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Photo { get; set; }

        public int SpecializationId { get; set; } // FK ke Specialization
        public Specialization? Specialization { get; set; } // Navigasi ke spesialisasi

        public int ProviderId { get; set; }
        public Provider? Provider { get; set; }

        public string? LicenseNumber { get; set; }
        public decimal? ConsultationFee { get; set; }
        public string? Biography { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<DoctorSchedule>? DoctorSchedules { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }
}
