namespace UIM.Core.Models.Dtos.Idea;

public class IdeaDetailsResponse : IdeaDto, IResponse
{
    public string Id { get; set; } = default!;
    public UserDetailsResponse User { get; set; } = default!;
    public DateTime CreatedDate { get; set; } = default!;
    public DateTime ModifiedDate { get; set; } = default!;
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
}