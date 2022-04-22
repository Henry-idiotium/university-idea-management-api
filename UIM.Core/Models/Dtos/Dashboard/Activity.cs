namespace UIM.Core.Models.Dtos.Dashboard;

public class MonthActivity
{
    public DateTime Date { get; set; }
    public int TotalComments { get; set; }
    public int TotalDislikes { get; set; }
    public int TotalIdeas { get; set; }
    public int TotalLikes { get; set; }
    public int TotalViews { get; set; }
}
