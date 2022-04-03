namespace UIM.Core.Helpers.Mappers;

public class AttachmentProfile : Profile
{
    public AttachmentProfile()
    {
        CreateMap<Attachment, AttachmentDetailsResponse>()
            .ForMember(
                dest => dest.IdeaId,
                opt => opt.MapFrom(src => EncryptHelpers.EncodeBase64Url(src.IdeaId))
            );

        CreateMap<ModifyAttachmentRequest, Attachment>()
            .ForMember(
                dest => dest.IdeaId,
                opt => opt.MapFrom(src => EncryptHelpers.DecodeBase64Url(src.IdeaId))
            );
    }
}
