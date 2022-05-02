using Sieve.Attributes;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace UIM.Core.Common.Entity;

public abstract class BaseEntity : IBaseEntity
{
    private DateTime? createdDate;

    [Sieve(CanFilter = true, CanSort = true)]
    [DataType(DataType.DateTime)]
    public DateTime CreatedDate
    {
        get => createdDate ?? DateTime.Now;
        set => createdDate = value;
    }

    [Sieve(CanFilter = true, CanSort = true)]
    [DataType(DataType.DateTime)]
    public DateTime ModifiedDate { get; set; } = DateTime.Now;

    [Sieve(CanFilter = true, CanSort = true)]
    public string? CreatedBy { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string? ModifiedBy { get; set; }
}

public abstract class Entity : BaseEntity, IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = default!;
}
