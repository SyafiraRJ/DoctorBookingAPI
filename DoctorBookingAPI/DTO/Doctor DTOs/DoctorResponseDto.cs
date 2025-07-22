namespace DoctorBookingAPI.DTO.Doctor_DTOs
{
    public class DoctorResponseDto
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Photo { get; set; }
        public string LicenseNumber { get; set; }
        public decimal ConsultationFee { get; set; }
        public string Biography { get; set; }
        public string SpecializationName { get; set; }
        public string ProviderName { get; set; }
    }

}
