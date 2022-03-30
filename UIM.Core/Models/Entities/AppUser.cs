namespace UIM.Core.Models.Entities;

public class AppUser : IdentityUser
{
    public string FullName { get; set; } = default!;
    public string? DepartmentId { get; set; }
    public Gender? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public bool IsDefaultPassword { get; set; } = true;

    private DateTime? createdDate;
    public DateTime CreatedDate
    {
        get => createdDate ?? DateTime.Now;
        set => createdDate = value;
    }

    public virtual Department? Department { get; set; }
    public virtual ICollection<Like>? Likes { get; set; }
    public virtual ICollection<View>? Views { get; set; }
    public virtual ICollection<Idea>? Ideas { get; set; }
    public virtual List<RefreshToken> RefreshTokens { get; set; } = default!;
}