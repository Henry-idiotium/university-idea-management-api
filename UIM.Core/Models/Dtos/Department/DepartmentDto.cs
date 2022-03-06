using System.ComponentModel.DataAnnotations;

namespace UIM.Core.Models.Dtos.Department
{
    public class DepartmentDto
    {
        [Required] public string Name { get; set; }
    }
}