using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Dto_s.Requests
{
    public class CarDTO
    {
        public string Registration { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public bool CanSmoke { get; set; }
    }
}
