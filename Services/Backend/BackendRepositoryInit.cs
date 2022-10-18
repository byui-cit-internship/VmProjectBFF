using vmProjectBFF.DTO;

namespace vmProjectBFF.Services
{
    public partial class BackendRepository : IBackendRepository
    {
        protected readonly ILogger<BackendRepository> _logger;
        protected BffResponse _lastResponse;

        public readonly IBackendHttpClient _backendHttpClient;
        public IBackendHttpClient BackendHttpClient { get { return _backendHttpClient; } }

        public BackendRepository(
            ILogger<BackendRepository> logger,
            IBackendHttpClient backendHttpClient)
        {
            _logger = logger;
            _backendHttpClient = backendHttpClient;
        }
    }
}