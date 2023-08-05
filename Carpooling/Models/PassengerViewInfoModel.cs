using CarPooling.Data.Models;

namespace Carpooling.Models
{
    public class PassengerViewInfoModel
    {
        public string? Username { get; set; }
        public decimal? AverageRating { get; set; }
        public IEnumerable<Feedback>? Feedbacks { get; set; }

    }
}
