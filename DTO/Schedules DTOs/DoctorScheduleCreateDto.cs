namespace DoctorBookingAPI.DTO.Schedules_DTOs
{
    public class DoctorScheduleCreateDto
    {
        public int DoctorID { get; set; }
        public byte DayOfWeek { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int SlotDuration { get; set; }
        public bool IsActive { get; set; }
    }
}
