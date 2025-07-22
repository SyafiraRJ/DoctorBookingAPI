    namespace DoctorBookingAPI.DTO
{
    public class DoctorListDto
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; }
        public string Photo { get; set; }
        public string SpecializationName { get; set; }
        public decimal ConsultationFee { get; set; }
        public string Rating { get; set; }
    }

}
