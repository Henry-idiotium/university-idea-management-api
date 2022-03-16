namespace UIM.Core.Helpers.Mappers;
public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDetailsResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(
                src => EncryptHelpers.EncodeBase64Url(src.Id)));

        CreateMap<CreateCategoryRequest, Category>();
        CreateMap<UpdateCategoryRequest, Category>();
    }
}