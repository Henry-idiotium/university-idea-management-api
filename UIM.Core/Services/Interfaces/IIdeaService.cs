namespace UIM.Core.Services.Interfaces;

public interface IIdeaService
    : IService<string, CreateIdeaRequest, UpdateIdeaRequest, IdeaDetailsResponse>
{

}