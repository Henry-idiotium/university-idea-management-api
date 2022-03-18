namespace UIM.Core.Helpers.Mappers;

public class SubmissionProfile : Profile
{
    public SubmissionProfile()
    {
        CreateMap<Submission, SubmissionDetailsResponse>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => EncryptHelpers.EncodeBase64Url(src.Id)));

        CreateMap<SubmissionDetailsResponse, Submission>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => EncryptHelpers.DecodeBase64Url(src.Id)));

        CreateMap<CreateSubmissionRequest, Submission>();

        CreateMap<UpdateSubmissionRequest, Submission>();
    }
}