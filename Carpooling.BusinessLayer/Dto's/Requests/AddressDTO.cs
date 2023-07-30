using CarPooling.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Dto_s.Requests
{
    public class AddressDTO
    {
        public string City { get; set; }    
        public string Details { get; set; }
        public string Country { get; set; }
    }
}
