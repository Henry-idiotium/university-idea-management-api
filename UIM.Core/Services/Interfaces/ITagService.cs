namespace UIM.Core.Services.Interfaces;

public interface ITagService
{
    Task CreateAsync(CreateTagRequest request);
    Task EditAsync(UpdateTagRequest request);
    IEnumerable<SimpleTagResponse> FindAll();
    Task<SieveResponse> FindAsync(SieveModel model);
    Task<TagDetailsResponse> FindByIdAsync(string entityId);
    Task<TagDetailsResponse> FindByNameAsync(string name);
    Task RemoveAsync(string entityId);
}
