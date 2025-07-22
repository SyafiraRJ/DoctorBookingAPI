using System.ComponentModel.DataAnnotations;

namespace DoctorBookingAPI.Model
{
    public class DoctorSchedule
    {
        [Key]
        public int SchedulesID { get; set; }
        public int DoctorID { get; set; }
        public byte DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int? SlotDuration { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

    }

}
