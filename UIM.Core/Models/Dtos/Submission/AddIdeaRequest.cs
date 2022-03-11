namespace UIM.Core.Models.Dtos.Submission;

public class AddIdeaRequest
{
    [Required] public string SubmissionId { get; set; } = default!;
    [Required] public string IdeaId { get; set; } = default!;
}