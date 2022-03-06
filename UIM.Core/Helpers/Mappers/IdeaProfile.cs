using System;
using AutoMapper;
using UIM.Core.Models.Dtos.Idea;
using UIM.Core.Models.Entities;

namespace UIM.Core.Helpers.Mappers
{
    public class IdeaProfile : Profile
    {
        public IdeaProfile()
        {
            CreateMap<UpdateIdeaRequest, Idea>();

            CreateMap<Idea, IdeaDetailsResponse>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => EncryptHelpers.EncodeBase64Url(src.Id)));

            CreateMap<IdeaDetailsResponse, Idea>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => EncryptHelpers.DecodeBase64Url(src.Id)));

            CreateMap<CreateIdeaRequest, Idea>()
                .ForMember(
                    dest => dest.UserId,
                    opt => opt.MapFrom(src => EncryptHelpers.DecodeBase64Url(src.UserId)));
        }
    }
}