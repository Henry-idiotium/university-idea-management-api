namespace UIM.Core.Models.Dtos.User;

public abstract class UserDto
{
    public string? UserName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    [Required] public string FullName { get; set; } = default!;
    [Required] public string Role { get; set; } = default!;
    [Required] public string? Department { get; set; }
}