namespace UIM.Core.Helpers.Mappers;

public class IdeaProfile : Profile
{
    public IdeaProfile()
    {
        CreateMap<Idea, IdeaDetailsResponse>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => EncryptHelpers.EncodeBase64Url(src.Id)));

        CreateMap<UpdateIdeaRequest, Idea>();
        CreateMap<CreateIdeaRequest, Idea>();
    }
}