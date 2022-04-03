namespace UIM.Core.Models.Dtos.Attachment;

public abstract class AttachmentDto
{
    public virtual string IdeaId { get; set; } = default!;
    public virtual string Name { get; set; } = default!;
    public virtual string FileId { get; set; } = default!;
    public virtual string Url { get; set; } = default!;
}
