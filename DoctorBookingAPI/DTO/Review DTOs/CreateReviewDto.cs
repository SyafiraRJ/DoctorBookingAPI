namespace DoctorBookingAPI.DTO
{
    public class ReviewCreateDto
    {
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public int AppointmentId { get; set; }
        public double Rating { get; set; }  // 1-5
        public string Comment { get; set; }
    }
}
