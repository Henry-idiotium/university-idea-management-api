namespace UIM.Core.Models.Dtos.Role;

public class RoleDetailsResponse : IResponse
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
}