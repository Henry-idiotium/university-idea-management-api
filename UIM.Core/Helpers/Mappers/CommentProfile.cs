using UIM.Core.Models.Dtos.Like;

namespace UIM.Core.Helpers.Mappers;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CommentDetailsResponse>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => EncryptHelpers.EncodeBase64Url(src.Id))
            );

        CreateMap<CreateCommentRequest, Comment>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now));

        CreateMap<UpdateCommentRequest, Comment>()
            .ForSourceMember(src => src.Id, opt => opt.DoNotValidate())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now));

        CreateMap<CreateLikeRequest, Like>();
    }
}
