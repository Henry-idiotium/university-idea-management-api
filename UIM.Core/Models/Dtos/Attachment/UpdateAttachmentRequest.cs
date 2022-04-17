namespace UIM.Core.Models.Dtos.Attachment;

public class UpdateAttachmentRequest
{
    public byte[]? Data { get; set; }
    public string? FileId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Mime { get; set; }
}
