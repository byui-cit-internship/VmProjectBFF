using VmProjectBFF.DTO;

namespace VmProjectBFF.Services
{
    public partial class BackendRepository : IBackendRepository
    {
        protected readonly ILogger<BackendRepository> _logger;
        protected BffResponse _lastResponse;

        public readonly IBackendHttpClient _backendHttpClient;

        public IVCenterRepository _vCenter;
        public IBackendHttpClient BackendHttpClient { get { return _backendHttpClient; } }

        public BackendRepository(
            ILogger<BackendRepository> logger,
            IBackendHttpClient backendHttpClient, IVCenterRepository vCenter
            )
        {
            _logger = logger;
            _backendHttpClient = backendHttpClient;
            _vCenter = vCenter;
        }
    }
}