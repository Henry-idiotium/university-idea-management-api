namespace UIM.Core.Helpers.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<AppUser, UserDetailsResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(
                src => EncryptHelpers.EncodeBase64Url(src.Id)));

        CreateMap<CreateUserRequest, AppUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(
                src => src.UserName ?? src.Email))
            .ForMember(dest => dest.Likes, opt => opt.Ignore())
            .ForMember(dest => dest.Views, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore());

        CreateMap<UpdateUserRequest, AppUser>()
            .ForMember(dest => dest.Likes, opt => opt.Ignore())
            .ForMember(dest => dest.Views, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore());
    }
}