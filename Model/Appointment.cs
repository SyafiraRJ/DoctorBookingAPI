namespace DoctorBookingAPI.Model
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int UserId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Status { get; set; }
        public string? PatientNotes { get; set; }
        public string? DoctorNotes { get; set; }
        public decimal? ConsultationFee { get; set; }
        public string? QueueNumber { get; set; }
        //public int? PaymentId { get; set; }
        public string? Symptoms { get; set; } // untuk QR appointment

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public User? User { get; set; }
        public Doctor? Doctor { get; set; }
        //public Payment? Payment { get; set; }
    }

}
