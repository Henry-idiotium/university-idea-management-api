namespace UIM.Core.Models.Dtos.Attachment;

public class UploadAttachmentRequest
{
    public byte[]? Data { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Mime { get; set; }
}
