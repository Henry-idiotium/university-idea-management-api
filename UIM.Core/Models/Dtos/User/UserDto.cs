namespace UIM.Core.Models.Dtos.User;

public abstract class UserDto
{
    public string? UserName { get; set; }
    [Required] public string FullName { get; set; } = default!;
    public DateTime? DateOfBirth { get; set; }
}