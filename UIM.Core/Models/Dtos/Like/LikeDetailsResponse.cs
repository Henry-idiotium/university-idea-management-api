namespace UIM.Core.Models.Dtos.Like;

public class LikeDetailsResponse
{
    public bool IsLike { get; set; }
    public DateTime CreatedDate { get; set; }
    public SimpleUserResponse? User { get; set; }
    public SimpleIdeaResponse? Idea { get; set; }
}
