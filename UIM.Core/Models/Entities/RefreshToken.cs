namespace UIM.Core.Models.Entities;

public class RefreshToken : Entity
{
    public string Token { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public DateTime ExpiredDate { get; set; }
    public DateTime? RevokedDate { get; set; }
    public string? ReplacedByToken { get; set; }
    public string? ReasonRevoked { get; set; }
    public bool IsExpired => DateTime.Now >= ExpiredDate;
    public bool IsRevoked => RevokedDate != null;
    public bool IsActive => !IsRevoked && !IsExpired;

    public virtual AppUser User { get; set; } = default!;
}