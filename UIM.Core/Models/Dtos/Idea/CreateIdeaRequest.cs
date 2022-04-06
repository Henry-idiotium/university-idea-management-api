using TJS = System.Text.Json.Serialization;

namespace UIM.Core.Models.Dtos.Idea;

public class CreateIdeaRequest : IdeaDto
{
    [TJS.JsonIgnore]
    public string? UserId { get; set; }
    [Required]
    public string SubmissionId { get; set; } = default!;
    public List<UploadAttachmentRequest>? Attachments { get; set; }
}
