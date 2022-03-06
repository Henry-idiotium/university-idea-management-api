using AutoMapper;
using UIM.Core.Models.Dtos.User;
using UIM.Core.Models.Entities;

namespace UIM.Core.Helpers.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, UserDetailsResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(
                    src => EncryptHelpers.EncodeBase64Url(src.Id)));

            CreateMap<CreateUserRequest, AppUser>();

            CreateMap<UpdateUserRequest, AppUser>();
        }
    }
}