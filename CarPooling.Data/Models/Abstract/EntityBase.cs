namespace CarPooling.Data.Models.Abstract
{
    public abstract class EntityBase
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime DeleteOn { get; set; }
    }
}
