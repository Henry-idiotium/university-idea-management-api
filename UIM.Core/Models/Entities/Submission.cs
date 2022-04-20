namespace UIM.Core.Models.Entities;

public class Submission : Entity
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime InitialDate { get; set; }
    public DateTime FinalDate { get; set; }
    public bool? IsFullyClose =>
        DateTime.Now >= InitialDate
            ? !(DateTime.Now > InitialDate && DateTime.Now < FinalDate)
            : null;

    public virtual ICollection<Idea>? Ideas { get; set; }
}
