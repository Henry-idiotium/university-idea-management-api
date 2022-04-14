namespace UIM.Core.Models.Dtos.User;

public abstract class UserDto
{
    [Required]
    public string FullName { get; set; } = default!;
    [Required]
    public string Role { get; set; } = default!;
    [Required]
    public string? Department { get; set; }
    public string? Phone { get; set; }
    public string? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
