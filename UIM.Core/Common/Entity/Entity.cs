using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace UIM.Core.Common.Entity;

public abstract class BaseEntity : IBaseEntity
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

public abstract class Entity : BaseEntity, IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = default!;
}