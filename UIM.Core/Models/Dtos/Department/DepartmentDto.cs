namespace UIM.Core.Models.Dtos.Department;

public class DepartmentDto
{
    [Required] public virtual string Name { get; set; } = default!;
}