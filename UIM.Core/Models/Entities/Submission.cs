namespace UIM.Core.Models.Entities;

public class Submission : Entity
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime InitialDate { get; set; }
    public DateTime FinalDate { get; set; }
    public bool IsActive { get; set; }

    public virtual ICollection<Idea>? Ideas { get; set; }
}