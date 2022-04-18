namespace UIM.Core.Models.Dtos.Idea;

public class MediumIdeaResponse
{
    public string? Id { get; set; }
    public virtual string Title { get; set; } = default!;
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public bool? RequesterIsLike { get; set; }
}
