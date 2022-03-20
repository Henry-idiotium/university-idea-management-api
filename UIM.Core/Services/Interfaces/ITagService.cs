namespace UIM.Core.Services.Interfaces;

public interface ITagService
    : IReadService<TagDetailsResponse>
    , IModifyService<CreateTagRequest, UpdateTagRequest>
{

}