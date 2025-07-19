using DoctorBookingAPI.DTO.Schedules_DTOs;

namespace DoctorBookingAPI.DTO
{
    public class DoctorDetailDto
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; }
        public string Photo { get; set; }
        public string SpecializationName { get; set; }
        public string ProviderName { get; set; }
        public string ProviderAddress { get; set; }
        public string GoogleMapsLink { get; set; }
        public decimal ConsultationFee { get; set; }
        public string Biography { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public List<ReviewDto> Reviews { get; set; }
        public List<DoctorScheduleDto> Schedules { get; set; }
    }

}
