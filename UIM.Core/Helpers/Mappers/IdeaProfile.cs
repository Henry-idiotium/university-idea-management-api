namespace UIM.Core.Helpers.Mappers;

public class IdeaProfile : Profile
{
    public IdeaProfile()
    {
        CreateMap<Idea, IdeaDetailsResponse>()
            .ForMember(
                dest => dest.CommentsCount,
                opt => opt.MapFrom(src => src.Comments != null ? src.Comments.Count : 0)
            )
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => EncryptHelpers.EncodeBase64Url(src.Id))
            );

        CreateMap<Idea, SimpleIdeaResponse>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => EncryptHelpers.EncodeBase64Url(src.Id))
            );

        CreateMap<UpdateIdeaRequest, Idea>()
            .ForSourceMember(src => src.Id, opt => opt.DoNotValidate())
            .ForSourceMember(dest => dest.Tags, opt => opt.DoNotValidate())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now));

        CreateMap<CreateIdeaRequest, Idea>()
            .ForSourceMember(dest => dest.Tags, opt => opt.DoNotValidate())
            .ForSourceMember(dest => dest.Attachments, opt => opt.DoNotValidate())
            .ForMember(dest => dest.Attachments, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now));
    }
}
