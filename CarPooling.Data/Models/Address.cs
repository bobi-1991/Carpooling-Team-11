using CarPooling.Data.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Models
{
    public class Address:LocationEntity
    {
        public Address()
        {
        }

        public int AddressNumber { get; set; }
        public int CityId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public City City { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Travel> Travels { get; set; }
    }
}
