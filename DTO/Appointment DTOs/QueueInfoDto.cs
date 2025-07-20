namespace DoctorBookingAPI.DTO.Appointment_DTOs
{

    public class QueueInfoDto
    {
        public string QueueNumber { get; set; }
        public int EstimatedWaitTime { get; set; }
        public int CurrentQueue { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }

        // Optional additions:
        public int TotalQueue { get; set; }
        public string Status { get; set; }
        public string CurrentlyServing { get; set; }
    }

}
