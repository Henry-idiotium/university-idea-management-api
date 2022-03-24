namespace UIM.Core.Common.Service;

public interface IReadService<TDetails>
    where TDetails : IResponse
{
    Task<SieveResponse> FindAsync(SieveModel model);
    Task<TDetails> FindByIdAsync(string entityId);
}