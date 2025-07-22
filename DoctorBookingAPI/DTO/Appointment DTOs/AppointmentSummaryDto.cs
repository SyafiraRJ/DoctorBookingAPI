namespace DoctorBookingAPI.DTO.Appointment_DTOs
{
    public class AppointmentSummaryDto
    {
        public int AppointmentId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorPhoto { get; set; }
        public string SpecializationName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string Status { get; set; }
        public string ProviderName { get; set; }
        public string QueueNumber { get; set; }
        public decimal ConsultationFee { get; set; }
        public string? PatientNotes { get; set; } // TAMBAH INI
    }
}
