using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Models;
using Microsoft.Extensions.Hosting;

namespace Carpooling.Models
{
    public class HomeViewModel
    {
        //  public List<UserResponse> Users { get; set; }

        public IEnumerable<User> TopTravelOrganizers { get; set; }
        public IEnumerable<User> TopPassengers { get; set; }
        public int TotalUsersCount { get; set; }
        public int TotalTravelCount { get; set; }
    }
}
