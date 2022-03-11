namespace UIM.Core.Common;

public abstract class Service
{
    protected IMapper _mapper;
    protected SieveOptions _sieveOptions;
    protected SieveProcessor _sieveProcessor;
    protected IUnitOfWork _unitOfWork;

    protected Service(
        IMapper mapper,
        IOptions<SieveOptions> sieveOptions,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _sieveOptions = sieveOptions.Value;
        _sieveProcessor = sieveProcessor;
        _unitOfWork = unitOfWork;
    }
}