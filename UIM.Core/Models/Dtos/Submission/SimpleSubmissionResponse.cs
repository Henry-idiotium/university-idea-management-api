namespace UIM.Core.Models.Dtos.Submission;

public class SimpleSubmissionResponse
{
    public string? Id { get; set; }
    public virtual string? Title { get; set; }
    public virtual string? Description { get; set; }
    public virtual bool? IsActive { get; set; }
}
