using CarPooling.Data.Models.Abstract;
using System.Text.Json.Serialization;

namespace CarPooling.Data.Models
{
    public class Country : EntityBase
    {
        public string Name { get; set; }
        [JsonIgnore]
        public List<Address>? Addresses { get; set; }
    }
}
