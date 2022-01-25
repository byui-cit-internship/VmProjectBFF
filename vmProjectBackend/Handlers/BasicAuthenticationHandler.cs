
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
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

        // BAsic Authentication needs contructor and this is it below
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            DatabaseContext context,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            // intialize the context
            _context = context;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            // make sure that authorization tag is in header
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Authorization tag and bearer token was not found");
            }

            try
            {
                // Read the authorization header
                var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                Logger.LogInformation("this is header value: " + authenticationHeaderValue);

                var validPayload = await GoogleJsonWebSignature.ValidateAsync(authenticationHeaderValue.Parameter);
                string validemail = validPayload.Email;
                // get users from the db and check if any match with what was send through the request
                User user = _context.Users.Where(user => user.Email == validemail).FirstOrDefault();
                // If we get no user
                if (user == null)
                {
                    AuthenticateResult.Fail("Invalid authorization token");
                }
                else
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, user.Email) };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }

            }
            catch (Exception)
            {
                return AuthenticateResult.Fail("Error");
            }

            return AuthenticateResult.Fail("Need to implement");
        }
    }
}