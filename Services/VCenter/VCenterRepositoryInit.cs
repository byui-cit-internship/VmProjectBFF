using vmProjectBFF.DTO;

namespace vmProjectBFF.Services
{
    public partial class VCenterRepository : IVCenterRepository
    {
        protected readonly ILogger<VCenterRepository> _logger;
        protected BffResponse _lastResponse;

        public readonly IVCenterHttpClient _vCenterHttpClient;
        public IVCenterHttpClient VCenterHttpClient { get { return _vCenterHttpClient; } }

        public VCenterRepository(
            ILogger<VCenterRepository> logger,
            IVCenterHttpClient vCenterHttpClient)
        {
            _logger = logger;
            _vCenterHttpClient = vCenterHttpClient;
        }
    }
}
