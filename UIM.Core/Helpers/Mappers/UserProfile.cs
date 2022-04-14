namespace UIM.Core.Helpers.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<AppUser, UserDetailsResponse>()
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForSourceMember(dest => dest.Department, opt => opt.DoNotValidate())
            .ForMember(
                dest => dest.Gender,
                opt =>
                    opt.MapFrom(
                        src => src.Gender != null ? Enum.GetName(typeof(Gender), src.Gender) : null
                    )
            )
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => EncryptHelpers.EncodeBase64Url(src.Id))
            );

        CreateMap<CreateUserRequest, AppUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Likes, opt => opt.Ignore())
            .ForMember(dest => dest.Views, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForSourceMember(src => src.Department, opt => opt.DoNotValidate())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.MapFrom(src => true));

        CreateMap<UpdateUserRequest, AppUser>()
            .ForSourceMember(src => src.Id, opt => opt.DoNotValidate())
            .ForMember(dest => dest.Likes, opt => opt.Ignore())
            .ForMember(dest => dest.Views, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName.ToLower()));
    }
}
