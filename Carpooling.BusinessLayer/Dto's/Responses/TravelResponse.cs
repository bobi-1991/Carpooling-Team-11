using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Service.Dto_s.Responses
{
    public class TravelResponse
    {
        public string StartLocationName { get; set; }
        public string DestinationName { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsComplete { get; set; }
        public string CarRegistration { get; set; }
    }
}
