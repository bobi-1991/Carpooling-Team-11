using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Service.Dto_s.Responses
{
    public class UserResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        // Optional? |
        
         public ICollection<Review> Reviews { get; set; }
        public ICollection<Travel> TravelHistory { get; set; }
        public ICollection<Car> Cars { get; set; }

        // Ново пропърти за среден рейтинг на юзъра?
        public decimal AverageRating { get; set; }
    }
}
