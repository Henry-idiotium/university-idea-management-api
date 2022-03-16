namespace UIM.Core.Models.Dtos.User;

public class CreateUserRequest : UserDto, ICreateRequest
{
    [Required] public string Email { get; set; } = default!;
    [Required] public string RoleId { get; set; } = default!;
    [Required] public string DepartmentId { get; set; } = default!;
}