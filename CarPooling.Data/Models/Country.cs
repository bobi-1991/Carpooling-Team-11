using CarPooling.Data.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Models
{
    public class Country : LocationEntity
    {
        public Country()
        {
        }

        public virtual ICollection<City> Cities { get; set; } = new List<City>();
        public ICollection<Address> Addresses { get; set;} = new List<Address>();
    }
}
