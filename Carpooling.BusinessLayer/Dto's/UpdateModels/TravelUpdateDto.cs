using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Dto_s.UpdateModels
{
    public class TravelUpdateDto
    {
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int StartLocationId { get; set; }
        public int DestionationId { get; set; }
        public int AvailableSeats { get; set; }
        public int CarId { get; set; }
    }
}
