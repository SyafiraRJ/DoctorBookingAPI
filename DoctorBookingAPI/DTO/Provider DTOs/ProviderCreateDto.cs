namespace DoctorBookingAPI.DTO.Provider_DTOs
{
    public class ProviderCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        //public string GoogleMapsLink { get; set; } = string.Empty;
        public double Latitude { get; set; } = 0.0;
        public double Longitude { get; set; } = 0.0;


    }
}
