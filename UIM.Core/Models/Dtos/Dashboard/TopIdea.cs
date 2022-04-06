namespace UIM.Core.Models.Dtos.Dashboard;

// TODO: add mapper profile
public class TopIdea
{
    public SimpleIdea? Idea { get; set; }
    public int CommentNumber { get; set; }
}

public class SimpleIdea
{
    public string? Id { get; set; }
    public string? Title { get; set; }
}
