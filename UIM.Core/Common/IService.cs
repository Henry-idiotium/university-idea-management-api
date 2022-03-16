namespace UIM.Core.Common;

public interface IService<TCreate, TUpdate, TDetails>
    where TCreate : ICreateRequest
    where TUpdate : IUpdateRequest
    where TDetails : IResponse
{
    Task CreateAsync(TCreate request);
    Task EditAsync(string entityId, TUpdate request);
    Task<SieveResponse> FindAsync(SieveModel model);
    Task<TDetails> FindByIdAsync(string entityId);
    Task RemoveAsync(string entityId);
}