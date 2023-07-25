using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Service.Dto_s.Requests
{
    public class FeedbackRequest
    {
        public string AuthorId { get; set; }
        public string DriverId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}
