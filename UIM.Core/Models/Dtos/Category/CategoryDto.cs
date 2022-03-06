using System.ComponentModel.DataAnnotations;

namespace UIM.Core.Models.Dtos.Category
{
    public abstract class CategoryDto
    {
        [Required] public string Name { get; set; }
    }
}