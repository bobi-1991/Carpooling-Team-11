using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Service.Dto_s.Requests
{
    public class TravelRequest
    {
        public int StartLocationId { get; set; }
        public int DestionationId { get; set; }
        public DateTime DepartureTime { get; set; }
        public int AvaibleSeats { get; set; }
        public int CarId { get; set; }
}
}
