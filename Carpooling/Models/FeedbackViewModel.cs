using System.ComponentModel.DataAnnotations;

namespace Carpooling.Models
{
    public class FeedbackViewModel
    {
        public string Comment { get; set; }
        [RegularExpression(@"^(?:[1-5](\.\d{1,2})?)$", ErrorMessage = "Invalid rating. Rating must be between 1 and 5.")]
        public int Rating { get; set; }
        public string ParticipantId { get; set; }
        public int TravelId { get; set; }

    }
}
