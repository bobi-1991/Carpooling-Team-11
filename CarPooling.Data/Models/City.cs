using CarPooling.Data.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Models
{
    public class City:LocationEntity
    {

        public int CountryId { get; set; }
        public Country Country { get; set; }
        public bool IsDelete { get; set; } = false;
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
