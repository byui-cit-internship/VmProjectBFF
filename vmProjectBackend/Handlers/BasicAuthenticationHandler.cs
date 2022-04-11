
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
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
        private readonly DatabaseContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        ILogger Logger { get; } = AppLogger.CreateLogger<BasicAuthenticationHandler>();

        // BAsic Authentication needs contructor and this is it below
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            DatabaseContext context,
            ISystemClock clock,
            IHttpContextAccessor httpContextAccessor
            )
            : base(options, logger, encoder, clock)
        {
            // intialize the context
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //
            string sessionCookie = _httpContextAccessor.HttpContext.Request.Cookies["vima_session_cookie"];
            
            if (sessionCookie == null)
            {
                return AuthenticateResult.Fail("No session token");
            }
            else
            {
                User user = (from st in _context.SessionTokens
                             join at in _context.AccessTokens
                             on st.AccessTokenId equals at.AccessTokenId
                             join u in _context.Users
                             on at.UserId equals u.UserId
                             where st.SessionCookie == sessionCookie
                             select u).FirstOrDefault();
                if (user != null)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, user.Email) };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
                else
                {
                    return AuthenticateResult.Fail("Invalid authorization token");
                }
            }
        }
    }
}