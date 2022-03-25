namespace UIM.Core.Models.Dtos.Submission;

public abstract class SubmissionDto
{
    public virtual string? Title { get; set; }
    public virtual string? Description { get; set; }
    public virtual DateTime InitialDate { get; set; }
    public virtual DateTime FinalDate { get; set; }
    public virtual bool? IsActive { get; set; }
}