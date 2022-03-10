using AutoMapper;
using UIM.Core.Models.Dtos.Category;
using UIM.Core.Models.Entities;

namespace UIM.Core.Helpers.Mappers
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDetailsResponse>();
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();
        }
    }
}