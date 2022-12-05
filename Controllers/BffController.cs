using Microsoft.AspNetCore.Mvc;
using VmProjectBFF.DTO;
using VmProjectBFF.Services;

namespace VmProjectBFF.Controllers
{
    public class BffController : ControllerBase
    {
        protected readonly IAuthorization _authorization;
        protected readonly IBackendRepository _backend;
        protected readonly ICanvasRepository _canvas;
        protected readonly IConfiguration _configuration;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ILogger _logger;
        protected readonly IVCenterRepository _vCenter;

        protected readonly IBackendHttpClient _backendHttpClient;
        protected readonly ICanvasHttpClient _canvasHttpClient;
        protected readonly IVCenterHttpClient _vCenterHttpClient;
        
        protected BffResponse _lastResponse;

        public IHttpClientFactory HttpClientFactory { get; }

        public BffController(
            IAuthorization authorization,
            IBackendRepository backend,
            ICanvasRepository canvas,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger logger,
            IVCenterRepository vCenter
            )
        {
            _authorization = authorization;
            _backend = backend;
            _canvas = canvas;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _vCenter = vCenter;

            _backendHttpClient = _backend.BackendHttpClient;
            _vCenterHttpClient = _vCenter.VCenterHttpClient;

            HttpClientFactory = httpClientFactory;

            _httpContextAccessor.HttpContext.Response.RegisterForDispose(_vCenterHttpClient);
        }
    }
}
