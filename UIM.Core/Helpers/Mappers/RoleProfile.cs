namespace UIM.Core.Helpers.Mappers;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<IdentityRole, RoleDetailsResponse>();
    }
}