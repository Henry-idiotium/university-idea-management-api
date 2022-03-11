namespace UIM.Core.Common;

public interface IService<TIdentity, TCreate, TUpdate, TDetails>
    where TCreate : ICreateRequest
    where TUpdate : IUpdateRequest
    where TDetails : IResponse
{
    Task CreateAsync(TCreate request);
    Task EditAsync(TIdentity entityId, TUpdate request);
    Task<TableResponse> FindAsync(SieveModel model);
    Task<TDetails> FindByIdAsync(TIdentity entityId);
    Task RemoveAsync(TIdentity entityId);
}