namespace UIM.Core.Models.Dtos.Comment;

public class SieveCommentResponse
{
    public SieveCommentResponse(IEnumerable<CommentDetailsResponse> rows, int total)
    {
        Rows = rows;
        Total = total;
    }

    public IEnumerable<CommentDetailsResponse> Rows { get; set; } = default!;
    public int Total { get; set; }
}
