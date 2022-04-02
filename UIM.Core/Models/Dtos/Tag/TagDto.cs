namespace UIM.Core.Models.Dtos.Department;

public abstract class TagDto
{
    [Required]
    public virtual string Name { get; set; } = default!;
}
