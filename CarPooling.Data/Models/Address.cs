using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AddressNumber { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
