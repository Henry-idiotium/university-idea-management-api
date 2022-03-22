namespace UIM.Core.Models.Entities;

public class Department : Entity
{
    public Department(string name) => Name = name;

    public string Name { get; set; } = default!;

    public virtual ICollection<AppUser>? Users { get; set; }
}