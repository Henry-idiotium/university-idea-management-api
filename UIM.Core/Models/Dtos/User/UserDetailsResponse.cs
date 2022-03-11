namespace UIM.Core.Models.Dtos.User;

public class UserDetailsResponse : UserDto, IResponse
{
    public string Id { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? Department { get; set; }
    public string Role { get; set; } = default!;
}