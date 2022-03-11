using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace UIM.Core.Common
{
    public abstract class Entity : IEntity
    {
        private DateTime? createdDate;
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate
        {
            get => createdDate ?? DateTime.UtcNow;
            set => createdDate = value;
        }

        [DataType(DataType.DateTime)]
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }

    public abstract class Entity<T> : IEntity<T>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; } = default!;

        private DateTime? createdDate;
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate
        {
            get { return createdDate ?? DateTime.UtcNow; }
            set { createdDate = value; }
        }

        [DataType(DataType.DateTime)]
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }
}