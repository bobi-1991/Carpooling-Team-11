using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Models;

namespace Carpooling.Models
{
    public class TravelViewResponseWithId : TravelResponse
    {
        public string DriverId { get; set; }
        public int Id { get; set; }
        public IEnumerable<User> Participants { get; set; }
    }
}
