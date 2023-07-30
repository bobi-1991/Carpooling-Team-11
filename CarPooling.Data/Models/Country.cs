using CarPooling.Data.Models.Abstract;
using System.Text.Json.Serialization;

namespace CarPooling.Data.Models
{
    public class Country : EntityBase
    {
        public string Name { get; set; }
        public List<Address>? Addresses { get; set; }
    }
}
