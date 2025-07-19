namespace DoctorBookingAPI.DTO.Schedules_DTOs
{
    public class TimeSlotDto
    {
        public string Time { get; set; }
        public bool IsAvailable { get; set; }
        public string Session { get; set; } // "Morning" or "Afternoon"
    }

}
