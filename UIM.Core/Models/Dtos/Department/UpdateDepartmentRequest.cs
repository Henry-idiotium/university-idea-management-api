namespace UIM.Core.Models.Dtos.Department;

public class UpdateDepartmentRequest : IUpdateRequest
{
    [Required] public virtual string OldName { get; set; } = default!;
    [Required] public virtual string NewName { get; set; } = default!;
}