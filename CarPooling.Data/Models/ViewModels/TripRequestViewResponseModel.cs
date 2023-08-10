using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Models.ViewModels
{
    public class TripRequestViewResponseModel
    {
        public TripRequestViewResponseModel(int id, string passengerUsername, string driverUsername, string startLocationDetails, string endLoacitonDetails, DateTime departureTime, string status)
        {
            Id = id;
            PassengerUsername = passengerUsername;
            DriverUsername = driverUsername;
            StartLocationDetails = startLocationDetails;
            EndLocationDetails = endLoacitonDetails;
            DepartureTime = departureTime;
            Status = status;
        }
        public int Id { get; set; }
        public string PassengerUsername { get; set; }
        public string DriverUsername { get; set; }
        public string StartLocationDetails { get; set; }
        public string EndLocationDetails { get; set; }
        public DateTime DepartureTime { get; set; }
        public string Status { get; set; }
    }
}
