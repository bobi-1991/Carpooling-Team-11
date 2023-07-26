using CarPooling.Data.Models.Abstract;

namespace CarPooling.Data.Models
{
    public class Address : EntityBase
    {
        public string? City { get; set; }

        public string? Details { get; set; }

        public int CountryId { get; set; }

        public Country? Country { get; set; }

        //public List<Travel>? Travels { get; set; }
    }
}
