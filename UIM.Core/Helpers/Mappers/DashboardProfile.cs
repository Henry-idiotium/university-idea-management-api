namespace UIM.Core.Helpers.Mappers;

public class DashboardProfile : Profile
{
    public DashboardProfile()
    {
        CreateMap<Idea, TopIdea>()
            .ForMember(
                dest => dest.Idea!.Id,
                opt => opt.MapFrom(src => EncryptHelpers.EncodeBase64Url(src.Id))
            )
            .ForMember(dest => dest.Idea!.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(
                dest => dest.CommentNumber,
                opt => opt.MapFrom(src => src.Comments != null ? src.Comments.Count : 0)
            );
    }
}
