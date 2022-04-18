namespace UIM.Core.Models.Dtos.Submission;

public abstract class SubmissionDto
{
    [Required]
    public virtual string? Title { get; set; }
    public virtual string? Description { get; set; }
    [Required]
    public virtual DateTime InitialDate { get; set; }
    [Required]
    public virtual DateTime FinalDate { get; set; }
    public virtual bool? IsActive { get; set; }
}
