
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
            try
            {
                Logger.LogInformation(_httpContextAccessor.HttpContext.Session.GetString("id"));
                User user = (from st in _context.SessionTokens
                             join at in _context.AccessTokens
                             on st.AccessTokenId equals at.AccessTokenId
                             join u in _context.Users
                             on at.UserId equals u.UserId
                             select u).FirstOrDefault();
                /*
                AuthenticationHeaderValue authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                HttpClient httpClient = _httpClientFactory.CreateClient();

                // Authorization Token for the Canvas url that we are hitting, we need this for every courese
                // and we will grab it 
                httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {authenticationHeaderValue}");

                // contains our base Url where individula course_id is added
                // This URL enpoint gives a list of all the Student in that class : role_id= 3 list all the student for that Professor
                HttpResponseMessage response = await httpClient.GetAsync($"https://openidconnect.googleapis.com/v1/userinfo");
                // Read the authorization header
<<<<<<< HEAD
                var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                // Console.WriteLine("this is headervalue" + authenticationHeaderValue);
=======
                Logger.LogInformation("this is header value: " + authenticationHeaderValue);
>>>>>>> auth-ebe

                var validPayload = await GoogleJsonWebSignature.ValidateAsync(authenticationHeaderValue.Parameter);
                string validemail = validPayload.Email;
                // get users from the db and check if any match with what was send through the request
                User user = _context.Users.Where(user => user.Email == validemail).FirstOrDefault();
                // If we get no user
                */
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