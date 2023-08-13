using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Models
{
    [Table("TravelUser")]
    public class TravelUser
    {
        public int Id { get; set; }
        public string PassengersId { get; set; }
        public int PassengersTravelHistoryId { get; set; }
    }
}
