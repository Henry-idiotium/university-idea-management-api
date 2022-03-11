namespace UIM.Core.Helpers.Mappers;

public class SubmissionProfile : Profile
{
    public SubmissionProfile()
    {
        CreateMap<Submission, SubmissionDetailsResponse>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => EncryptHelpers.EncodeBase64Url(src.Id)))
            .ForMember(
                dest => dest.ModifiedBy,
                opt => opt.MapFrom(src => EncryptHelpers.EncodeBase64Url(src.ModifiedBy)));

        CreateMap<SubmissionDetailsResponse, Submission>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => EncryptHelpers.DecodeBase64Url(src.Id)))
            .ForMember(
                dest => dest.ModifiedBy,
                opt => opt.MapFrom(src => EncryptHelpers.DecodeBase64Url(src.ModifiedBy)));

        CreateMap<CreateSubmissionRequest, Submission>()
            .ForMember(
                dest => dest.ModifiedBy,
                opt => opt.MapFrom(src => EncryptHelpers.DecodeBase64Url(src.ModifiedBy)));

        CreateMap<UpdateSubmissionRequest, Submission>()
            .ForMember(
                dest => dest.ModifiedBy,
                opt => opt.MapFrom(src => EncryptHelpers.DecodeBase64Url(src.ModifiedBy)));
    }
}