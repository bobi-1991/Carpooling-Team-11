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

        // След интегрирането на Microsoft maps
       // public DateTime ArrivalTime { get; set; }

        public int AvaibleSeats { get; set; }
        public string CarRegistration { get; set; }
    }
}
