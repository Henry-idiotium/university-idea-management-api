namespace UIM.Core.Helpers.Mappers;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<Department, DepartmentDetailsResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(
                src => EncryptHelpers.EncodeBase64Url(src.Id)));

        CreateMap<CreateDepartmentRequest, Department>();
        CreateMap<UpdateDepartmentRequest, Department>();
    }
}