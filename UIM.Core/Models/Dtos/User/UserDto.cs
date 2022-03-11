namespace UIM.Core.Models.Dtos.User;

public abstract class UserDto
{
    [Required] public string UserName { get; set; } = default!;
    [Required] public string FullName { get; set; } = default!;
    public DateTime? DateOfBirth { get; set; }
}