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

        CreateMap<UploadAttachmentRequest, Attachment>();
        CreateMap<UpdateAttachmentRequest, Attachment>();
    }
}
