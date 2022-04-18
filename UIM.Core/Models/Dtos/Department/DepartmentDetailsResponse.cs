namespace UIM.Core.Models.Dtos.Department;

public class DepartmentDetailsResponse : DepartmentDto
{
    public string? Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}
