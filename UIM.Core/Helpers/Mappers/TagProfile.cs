namespace UIM.Core.Helpers.Mappers;
public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<Tag, TagDetailsResponse>();

        CreateMap<CreateTagRequest, Tag>();
        CreateMap<UpdateTagRequest, Tag>();
    }
}