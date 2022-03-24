namespace UIM.Core.Models.Dtos.Department;

public class DepartmentDetailsResponse : IResponse
{
    [Required] public string Name { get; set; } = default!;
}