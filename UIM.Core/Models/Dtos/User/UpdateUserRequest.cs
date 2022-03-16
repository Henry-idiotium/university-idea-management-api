namespace UIM.Core.Models.Dtos.User;

public class UpdateUserRequest : UserDto, IUpdateRequest
{
    [Required] public string Password { get; set; } = default!;
    [Required] public string ConfirmPassword { get; set; } = default!;
    [Required] public string RoleId { get; set; } = default!;
    [Required] public string DepartmentId { get; set; } = default!;
}