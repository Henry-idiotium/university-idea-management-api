namespace UIM.Core.Models.Entities;

public class Department : Entity
{
    public string Name { get; set; } = default!;

    public virtual ICollection<AppUser>? Users { get; set; }
}