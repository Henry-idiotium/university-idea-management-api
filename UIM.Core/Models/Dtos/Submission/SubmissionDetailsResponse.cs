namespace UIM.Core.Models.Dtos.Submission;

public class SubmissionDetailsResponse : SubmissionDto
{
    public string? Id { get; set; }
    public bool? IsFullyClose { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime ModifiedDate { get; set; }
}
