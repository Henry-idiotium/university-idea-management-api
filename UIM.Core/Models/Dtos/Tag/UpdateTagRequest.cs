namespace UIM.Core.Models.Dtos.Tag;

public class UpdateTagRequest : IUpdateRequest
{
    [Required] public string OldName { get; set; } = default!;
    [Required] public string NewName { get; set; } = default!;
}