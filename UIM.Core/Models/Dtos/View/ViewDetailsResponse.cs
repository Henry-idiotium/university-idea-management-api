namespace UIM.Core.Models.Dtos.View;

public class ViewDetailsResponse
{
    public SimpleIdeaResponse? Idea { get; set; }
    public SimpleUserResponse? User { get; set; }
    public DateTime CreatedDate { get; set; }
}
