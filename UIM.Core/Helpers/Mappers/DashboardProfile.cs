namespace UIM.Core.Helpers.Mappers;

public class DashboardProfile : Profile
{
    public DashboardProfile()
    {
        CreateMap<Idea, SimpleIdea>();
        CreateMap<Idea, TopIdea>()
            .ForMember(
                dest => dest.CommentNumber,
                opt => opt.MapFrom(src => src.Comments != null ? src.Comments.Count : 0)
            );
        
    }
}
