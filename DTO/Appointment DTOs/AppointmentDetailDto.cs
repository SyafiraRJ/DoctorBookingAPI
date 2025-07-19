namespace DoctorBookingAPI.DTO
{
    public class AppointmentDetailDto
    {
        public int AppointmentId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;

        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Status { get; set; }
        public string? PatientNotes { get; set; }
        public decimal? ConsultationFee { get; set; }
    }

}
