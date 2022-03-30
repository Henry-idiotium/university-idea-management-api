namespace UIM.Core.Helpers.Mappers;

public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<Tag, TagDetailsResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => EncryptHelpers.EncodeBase64Url(src.Id)));

        CreateMap<CreateTagRequest, Tag>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToLower()))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now));

        CreateMap<UpdateTagRequest, Tag>()
            .ForSourceMember(src => src.Id, opt => opt.DoNotValidate())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToLower()))
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now));
    }
}