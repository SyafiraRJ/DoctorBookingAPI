namespace DoctorBookingAPI.DTO.Appointment_DTOs
{

    public class QueueInfoDto
    {
        public string QueueNumber { get; set; }
        public int EstimatedWaitTime { get; set; }
        public int CurrentQueue { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
    }

}
