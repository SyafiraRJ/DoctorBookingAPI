namespace DoctorBookingAPI.DTO.Schedules_DTOs
{
    public class DoctorScheduleDto
    {
        public int SchedulesID { get; set; }
        public string DayOfWeek { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int SlotDuration { get; set; }
        public bool IsActive { get; set; }
    }

}
