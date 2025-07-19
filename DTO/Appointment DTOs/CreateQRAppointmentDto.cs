namespace DoctorBookingAPI.DTO.Appointment_DTOs
{
    public class CreateQRAppointmentDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public int SpecializationId { get; set; }
        public string Symptoms { get; set; }
        public string QRData { get; set; }
    }

}
