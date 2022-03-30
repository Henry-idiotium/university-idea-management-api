namespace UIM.Core.Models.Dtos.User;

public class UserDetailsResponse : UserDto, IResponse
{
    public string Id { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool IsDefaultPassword { get; set; }
    public string? Gender { get; set; }
    public string? PhoneNumber { get; set; }
}