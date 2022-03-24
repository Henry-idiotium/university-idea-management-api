namespace UIM.Core.Helpers.Mappers;

public class SimpleEntitiesProfile : Profile
{
    public SimpleEntitiesProfile()
    {
        CreateMap<Tag, TagDetailsResponse>();
        CreateMap<IdentityRole, RoleDetailsResponse>();
        CreateMap<Department, DepartmentDetailsResponse>();
    }
}