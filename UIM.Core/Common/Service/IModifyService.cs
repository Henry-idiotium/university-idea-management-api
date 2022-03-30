namespace UIM.Core.Common.Service;

public interface IModifyService<TCreate, TUpdate>
    where TCreate : ICreateRequest
    where TUpdate : IUpdateRequest
{
    Task CreateAsync(TCreate request);
    Task EditAsync(TUpdate request);
    Task RemoveAsync(string entityId);
}