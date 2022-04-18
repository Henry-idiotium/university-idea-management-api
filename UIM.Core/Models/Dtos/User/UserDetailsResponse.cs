namespace UIM.Core.Models.Dtos.User;

public class UserDetailsResponse : UserDto
{
    public string Id { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool IsDefaultPassword { get; set; }
}
