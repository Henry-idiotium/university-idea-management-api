namespace UIM.Core.Models.Dtos.Tag;

public class TagDetailsResponse : IResponse
{
    [Required] public string Name { get; set; } = default!;
}