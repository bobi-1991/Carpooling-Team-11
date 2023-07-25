using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Service.Dto_s.Responses
{
    public class TripRequestResponse
    {
        public string AuthorUsername { get; set; }
        public string DriverUsername { get; set; }
        public string Status { get; set; }
    }
}
