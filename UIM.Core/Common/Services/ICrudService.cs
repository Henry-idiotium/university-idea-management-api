namespace UIM.Core.Services.Interfaces;

public interface ICrudService<TCreate, TUpdate, TDetails>
    : IModifyService<TCreate, TUpdate>
    , IReadService<TDetails>
    where TCreate : ICreateRequest
    where TUpdate : IUpdateRequest
    where TDetails : IResponse
{

}