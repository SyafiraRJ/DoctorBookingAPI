namespace DoctorBookingAPI.DTO.Appointment_DTOs
{
    public class AppointmentUpdateDto
    {
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string? Status { get; set; }
        public string? PatientNotes { get; set; }
    }
}
