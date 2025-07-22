namespace DoctorBookingAPI.DTO.Appointment_DTOs
{
    public class CreateQRAppointmentDto
    {
        public int UserId { get; set; }  // User ID wajib, tapi kita cek manual
        public string? Symptoms { get; set; }
        public string? QRData { get; set; }
        public string? PatientNotes { get; set; }
    }
}
