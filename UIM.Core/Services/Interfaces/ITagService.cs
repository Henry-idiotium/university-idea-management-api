namespace UIM.Core.Services.Interfaces;

public interface ITagService
    : IService<
        CreateTagRequest,
        UpdateTagRequest,
        TagDetailsResponse>
{

}