namespace DoctorBookingAPI.DTO.Doctor_DTOs
{
    public class DoctorCreateDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Photo { get; set; }
        public int SpecializationId { get; set; }
        public int ProviderId { get; set; }
        public string LicenseNumber { get; set; }
        public decimal ConsultationFee { get; set; }
        public string Biography { get; set; }
        public bool IsActive { get; set; } = true;
    }

}
