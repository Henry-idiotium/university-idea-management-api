namespace UIM.Core.Models.Dtos.Idea;

public class CreateIdeaRequest : IdeaDto, ICreateRequest
{
    [Required] public string UserId { get; set; } = default!;
}