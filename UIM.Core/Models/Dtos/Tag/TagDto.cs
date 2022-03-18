namespace UIM.Core.Models.Dtos.Tag;

public abstract class TagDto
{
    [Required] public string Name { get; set; } = default!;
}