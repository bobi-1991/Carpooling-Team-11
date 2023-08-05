using CarPooling.Data.Models;

namespace Carpooling.Models
{
    public class DriverViewInfoModel
    {
        public string? Username { get; set; }
        public decimal? AverageRating { get; set; }
        public IEnumerable<Feedback>? Feedbacks { get; set; }

        //car info
        public string Capacity { get; set; }
        public string? CarModel { get; set; }
        public string? CarBrand { get; set; }
        public string? CarColor { get; set; }
        public string? Registration { get; set; }
        public bool? CanSmoke { get; set; }
    }
}
