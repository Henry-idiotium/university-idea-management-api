namespace UIM.Core.Common.Services;

public interface IReadService<TDetails>
    where TDetails : IResponse
{
    Task<SieveResponse> FindAsync(SieveModel model);
    Task<TDetails> FindByIdAsync(string entityId);
}