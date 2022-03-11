namespace UIM.Core.Models.Entities;

public class Department : Entity<int>
{
    public string Name { get; set; } = default!;

    public virtual ICollection<AppUser>? Users { get; set; }
}