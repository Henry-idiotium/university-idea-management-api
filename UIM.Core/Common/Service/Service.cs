namespace UIM.Core.Common.Service;

public abstract class Service
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
}