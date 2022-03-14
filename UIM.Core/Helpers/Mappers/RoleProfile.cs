namespace UIM.Core.Helpers.Mappers;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<IdentityRole, RoleDetailsResponse>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => EncryptHelpers.EncodeBase64Url(src.Id)));
    }
}