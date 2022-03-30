namespace UIM.Core.Models.Dtos.Department;

public abstract class DepartmentDto
{
    [Required] public virtual string Name { get; set; } = default!;
}