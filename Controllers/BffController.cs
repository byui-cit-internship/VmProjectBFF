using Microsoft.AspNetCore.Mvc;
using vmProjectBFF.DTO;
using vmProjectBFF.Services;

namespace vmProjectBFF.Controllers
{
    public class BffController : ControllerBase
    {
        protected readonly Authorization _auth;
        protected readonly BackendHttpClient _backend;
        protected readonly IConfiguration _configuration;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ILogger _logger;
        protected BffResponse _lastResponse;

        public IHttpClientFactory HttpClientFactory { get; }

        public BffController(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger logger)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _backend = new(
                _configuration,
                logger,
                GetVimaCookie(_httpContextAccessor));
            _auth = new(
                _backend,
                logger);
            HttpClientFactory = httpClientFactory;
        }

        protected static string GetVimaCookie(IHttpContextAccessor httpContextAccessor)
        {
            httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("vima-cookie", out string vimaCookie);
            return vimaCookie;
        }
    }
}
