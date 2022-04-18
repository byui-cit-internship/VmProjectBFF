
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;

namespace vmProjectBackend.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        // Inject the DBcontext into the handler so that we can compare te credentials
        private readonly ILogger<BasicAuthenticationHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        // BAsic Authentication needs contructor and this is it below
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            ILogger<BasicAuthenticationHandler> logger,
            ISystemClock clock,
            IHttpContextAccessor httpContextAccessor
            )
            : base(options, loggerFactory, encoder, clock)
        {
            // intialize the context
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            string serializedUser = _httpContextAccessor.HttpContext.Session.GetString("serializedUser");
            if (serializedUser != null)
            {
                User authenticatedUser = JsonConvert.DeserializeObject<User>(serializedUser);
                if (authenticatedUser != null)
                {
                    Claim[] claims = new[] { new Claim(ClaimTypes.Name, authenticatedUser.Email) };
                    ClaimsIdentity identity = new(claims, Scheme.Name);
                    ClaimsPrincipal principal = new(identity);
                    AuthenticationTicket ticket = new(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
                else
                {
                    return AuthenticateResult.Fail("Invalid authorization token");
                }
            }
            else
            {
                return AuthenticateResult.Fail("No session token");
            }
        }
    }
}