using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Service.Dto_s.Responses
{
    public class TripRequestResponse
    {
        public TripRequestResponse(string passengerUsername, string driverUsername,string startLocationDetails,string endLoacitonDetails, DateTime departureTime, string status)
        {
            PassengerUsername = passengerUsername;
            DriverUsername = driverUsername;
            StartLocationDetails = startLocationDetails;
            EndLocationDetails = endLoacitonDetails;
            DepartureTime = departureTime;
            Status = status;
        }
        public string PassengerUsername { get; set; }
        public string DriverUsername { get; set; }
        public string StartLocationDetails { get; set; }
        public string EndLocationDetails { get; set; }
        public DateTime DepartureTime { get; set; }
        public string Status { get; set; }
    }
}
