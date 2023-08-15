using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Service.Dto_s.Responses
{
    public class FeedbackDTO
    {
        public string GiverUsername { get; set; }
        public string ReceiverUsername { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public int TravelID { get; set; }
    }
}
