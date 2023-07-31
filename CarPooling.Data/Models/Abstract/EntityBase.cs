using System.Text.Json.Serialization;

namespace CarPooling.Data.Models.Abstract
{
    public abstract class EntityBase
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public bool IsDeleted { get; set; }
        [JsonIgnore]
        public DateTime CreatedOn { get; set; }
        [JsonIgnore]
        public DateTime UpdatedOn { get; set; }
        [JsonIgnore]
        public DateTime DeletedOn { get; set; }
    }
}
