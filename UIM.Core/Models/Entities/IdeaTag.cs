namespace UIM.Core.Models.Entities;

public class SubmissionTag : BaseEntity
{
    public string TagId { get; set; } = default!;
    public string SubmissionId { get; set; } = default!;

    public virtual Tag? Tag { get; set; }
    public virtual Submission? Submission { get; set; }
}