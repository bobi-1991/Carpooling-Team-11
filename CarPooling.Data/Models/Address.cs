using CarPooling.Data.Models.Abstract;
using System.Text.Json.Serialization;

namespace CarPooling.Data.Models
{
    public class Address : EntityBase
    {
        public string? City { get; set; }

        public string? Details { get; set; }
        [JsonIgnore]
        public int CountryId { get; set; }
        public Country? Country { get; set; }

        //public List<Travel>? Travels { get; set; }
    }
}
