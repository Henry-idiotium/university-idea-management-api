namespace UIM.Core.Models.Dtos.User;

public class CreateUserRequest : UserDto
{
    [Required]
    public string Email { get; set; } = default!;
}
