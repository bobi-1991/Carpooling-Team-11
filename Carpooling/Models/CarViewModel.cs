namespace Carpooling.Models
{
    public class CarViewModel
    {
        public string Registration { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public bool CanSmoke { get; set; }
    }
}
