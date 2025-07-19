namespace DoctorBookingAPI.DTO
{
    public class DoctorListDto
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; }
        public string Photo { get; set; }
        public string SpecializationName { get; set; }
        public string ProviderName { get; set; }
        public decimal ConsultationFee { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
    }

}
