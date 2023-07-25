using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Service.Dto_s.Responses
{
    public class FeedbackResponse
    {
        public string AuthorUsername { get; set; }
        public string DriverUsername { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}
