namespace UIM.Core.Services.Interfaces;

public interface ITagService
{
    Task CreateAsync(string name);
    Task EditAsync(UpdateTagRequest request);
    IEnumerable<TagDetailsResponse> FindAll();
    Task<TagDetailsResponse> FindByIdAsync(string entityId);
    Task<TagDetailsResponse> FindByNameAsync(string name);
    Task RemoveAsync(string name);
}