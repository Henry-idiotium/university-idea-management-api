namespace UIM.Core.Common;

public abstract class Service
{
    protected readonly IMapper _mapper;
    protected readonly SieveProcessor _sieveProcessor;
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly UserManager<AppUser> _userManager;

    protected Service(
        IMapper mapper,
        SieveProcessor sieveProcessor,
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager
    )
    {
        _mapper = mapper;
        _sieveProcessor = sieveProcessor;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }
}
