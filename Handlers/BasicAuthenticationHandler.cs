using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;
using vmProjectBFF.DTO;
using vmProjectBFF.Exceptions;
using vmProjectBFF.Models;
using vmProjectBFF.Services;

namespace vmProjectBFF.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        // Inject the DBcontext into the handler so that we can compare te credentials
        private readonly IBackendHttpClient _backendHttpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<BasicAuthenticationHandler> _logger;

        private BffResponse _lastResponse;

        // BAsic Authentication needs contructor and this is it below
        public BasicAuthenticationHandler(
            IBackendHttpClient backendHttpClient,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<BasicAuthenticationHandler> logger,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            ISystemClock clock
            )
            : base(
                  options,
                  loggerFactory,
                  encoder,
                  clock)
        {
            // intialize the context
            _backendHttpClient = backendHttpClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }



        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {

                _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("vima-cookie", out string vimaCookie);

                if (vimaCookie != null)
                {
                    _lastResponse = _backendHttpClient.Get($"api/v1/Token", new { sessionToken = vimaCookie });
                    User user = JsonConvert.DeserializeObject<User>(_lastResponse.Response);

                    if (user != null)
                    {
                        return SuccessResult(user.Email);
                    }
                }
                return AuthenticateResult.Fail("No session token");
            }
            catch (BffHttpException be)
            {
                return AuthenticateResult.Fail($"Failure to contact backend, returned message \"{be.Message}\"");
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.StackTrace);
                return AuthenticateResult.Fail($"Validating authentication token failed due to: {e.Message}");
            }
        }

        public AuthenticateResult SuccessResult(string name)
        {
            var claims = new[] { new Claim(ClaimTypes.Name, name) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}