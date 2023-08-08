using CarPooling.Data.Models;

namespace Carpooling.Models
{
    public class DriverViewInfoModel
    {
        public string? Username { get; set; }
        public decimal? AverageRating { get; set; }
        public IEnumerable<Feedback>? Feedbacks { get; set; }
        public IEnumerable<Car> Cars { get; set; }

    }
}
