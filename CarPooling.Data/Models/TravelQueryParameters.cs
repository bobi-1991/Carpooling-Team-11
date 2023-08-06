using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Models
{
    public class TravelQueryParameters
    {
        public string? DriverUsername { get; set; }
        public string? StartLocation { get; set; }
        public string? EndLocation { get; set; }
        public string? IsCompleted { get; set; }
        public int? AvailableSeats { get; set; }
        public string? SortBy { get; set; }


        public int PageSize { get; set; } = 3;
        public int PageNumber { get; set; } = 1;



    }
}
