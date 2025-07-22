namespace DoctorBookingAPI.DTO
{
    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public string PatientName { get; set; }  // diambil dari User.FullName
        public double Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
