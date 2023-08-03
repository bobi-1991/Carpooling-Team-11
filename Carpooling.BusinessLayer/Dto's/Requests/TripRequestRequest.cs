using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Service.Dto_s.Requests
{
    public class TripRequestRequest
    {
        public string PassengerId { get; set; }
        public string DriverId { get; set; }
        public int TravelId { get; set; }

    }
}
