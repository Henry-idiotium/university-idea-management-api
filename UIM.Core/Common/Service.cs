namespace UIM.Core.Common;

public abstract class Service<TCreate, TUpdate, TDetails>
    : IService<TCreate, TUpdate, TDetails>
    where TCreate : ICreateRequest
    where TUpdate : IUpdateRequest
    where TDetails : IResponse
{
    protected IMapper _mapper;
    protected SieveProcessor _sieveProcessor;
    protected IUnitOfWork _unitOfWork;

    protected Service(
        IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _sieveProcessor = sieveProcessor;
        _unitOfWork = unitOfWork;
    }

    public abstract Task CreateAsync(TCreate request);
    public abstract Task EditAsync(string entityId, TUpdate request);
    public abstract Task<SieveResponse> FindAsync(SieveModel model);
    public abstract Task<TDetails> FindByIdAsync(string entityId);
    public abstract Task RemoveAsync(string entityId);
}