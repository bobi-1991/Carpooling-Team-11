using CarPooling.Data.Models.Abstract;

namespace CarPooling.Data.Models
{
    public class Country : EntityBase
    {
        public string Name { get; set; }

        public List<Address>? Addresses { get; set; }
    }
}
