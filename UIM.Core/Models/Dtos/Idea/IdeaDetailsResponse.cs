namespace UIM.Core.Models.Dtos.Idea;

public class IdeaDetailsResponse : IdeaDto
{
    public string? Id { get; set; }
    public SubmissionDetailsResponse? Submission { get; set; }
    public UserDetailsResponse? User { get; set; }
    public virtual AttachmentDetailsResponse[]? Attachments { get; set; }
    public string? CreatedBy { get; set; }
    public int Views { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public bool? RequesterIsLike { get; set; }
    public int CommentsCount { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime ModifiedDate { get; set; }
}
