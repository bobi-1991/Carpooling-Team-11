using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Models.ViewModels
{
    public class PassengersListViewModel
    {
        public DateTime DepartureTime { get; set; }
        public string DepartureCity { get; set; }
        public string DepartureAddress { get; set; }
        public string ArrivalCity { get; set; }
        public string ArrivalAddress { get; set; }
        public string Username { get; set; }
        public decimal AverageRating { get; set; }

       

    }
}
