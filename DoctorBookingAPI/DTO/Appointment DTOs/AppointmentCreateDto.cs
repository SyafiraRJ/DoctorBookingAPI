namespace DoctorBookingAPI.DTO
{
    public class AppointmentCreateDto
    {
        public int UserId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string? Symptoms { get; set; }
        public string? PatientNotes { get; set; }

    }

}
