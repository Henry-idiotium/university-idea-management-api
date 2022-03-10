using AutoMapper;
using UIM.Core.Models.Dtos.Department;
using UIM.Core.Models.Entities;

namespace UIM.Core.Helpers.Mappers
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentDetailsResponse>();
            CreateMap<CreateDepartmentRequest, Department>();
            CreateMap<UpdateDepartmentRequest, Department>();
        }
    }
}