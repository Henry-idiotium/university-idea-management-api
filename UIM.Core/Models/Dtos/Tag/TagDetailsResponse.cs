namespace UIM.Core.Models.Dtos.Tag;

public class TagDetailsResponse : TagDto, IResponse
{
    public string? Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
}