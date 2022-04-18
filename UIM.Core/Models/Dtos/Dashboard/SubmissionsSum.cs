namespace UIM.Core.Models.Dtos.Dashboard;

public class SubmissionsSum
{
    public string? Month { get; set; }
    public int ActiveSubmissions { get; set; }
    public int InactiveSubmissions { get; set; }
}
