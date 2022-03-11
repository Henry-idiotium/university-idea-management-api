namespace UIM.Core.Models.Entities;

public class AppUser : IdentityUser
{
    public string FullName { get; set; } = default!;
    public int? DepartmentId { get; set; }
    public DateTime? DateOfBirth { get; set; }

    private DateTime? createdDate;
    public DateTime CreatedDate
    {
        get => createdDate ?? DateTime.UtcNow;
        set => createdDate = value;
    }

    public virtual Department? Department { get; set; }
    public virtual ICollection<Like>? Likes { get; set; }
    public virtual ICollection<View>? Views { get; set; }
    public virtual ICollection<Idea>? Ideas { get; set; }
    public virtual List<RefreshToken> RefreshTokens { get; set; } = default!;
}