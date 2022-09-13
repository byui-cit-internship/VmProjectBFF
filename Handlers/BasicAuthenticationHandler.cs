using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using vmProjectBFF.DTO;
using vmProjectBFF.Models;
using vmProjectBFF.Services;

namespace vmProjectBFF.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        // Inject the DBcontext into the handler so that we can compare te credentials
        private readonly Backend _backend;
        private readonly ILogger<BasicAuthenticationHandler> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private BackendResponse _lastResponse;

        // BAsic Authentication needs contructor and this is it below
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            ILogger<BasicAuthenticationHandler> logger,
            ISystemClock clock,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration
            )
            : base(options, loggerFactory, encoder, clock)
        {
            // intialize the context
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _backend = new(_httpContextAccessor, _logger, _configuration);
        }


        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                string sessionTokenValue = _httpContextAccessor.HttpContext.Session.GetString("sessionTokenValue");
                string serializedUser = _httpContextAccessor.HttpContext.Session.GetString("serializedUser");

                string storedCookie = _httpContextAccessor.HttpContext.Session.GetString("BFFSessionCookie");
                string requestCookie = _httpContextAccessor.HttpContext.Request.Cookies[".VMProjectBFF.Session"] != null ? $".VMProjectBFF.Session={_httpContextAccessor.HttpContext.Request.Cookies[".VMProjectBFF.Session"]}" : null;


                if (storedCookie == requestCookie && storedCookie != null)
                {
                    User authenticatedUser = JsonConvert.DeserializeObject<User>(serializedUser);
                    return SuccessResult(authenticatedUser.Email);
                }
                else if (storedCookie == null && requestCookie == null)
                {
                    return AuthenticateResult.Fail("No session cookie recieved or stored");
                }
                else if (requestCookie != null)
                {
                    if (storedCookie == null)
                    {
                        string[] cookieParts = requestCookie.Split('=', 2);
                        _lastResponse = _backend.Get($"api/v2/Cookie", new { cookieName = cookieParts[0], cookieValue = cookieParts[1], siteFrom = "BFF" });
                        Cookie dbCookie = JsonConvert.DeserializeObject<Cookie>(_lastResponse.Response);

                        if (dbCookie != null)
                        {
                            _lastResponse = _backend.Get($"api/v2/UserSession", new { cookieName = cookieParts[0], cookieValue = cookieParts[1], siteFrom = "BFF" });
                            UserSession userSession = JsonConvert.DeserializeObject<UserSession>(_lastResponse.Response);

                            _lastResponse = _backend.Get($"api/v2/Cookie", new { siteFrom = "BE", sessionTokenValue = userSession.SessionToken.SessionTokenValue.ToString() });
                            Cookie beCookie = JsonConvert.DeserializeObject<Cookie>(_lastResponse.Response);

                            _httpContextAccessor.HttpContext.Session.SetString("BESessionCookie", $"{beCookie.CookieName}={beCookie.CookieValue}");
                            _httpContextAccessor.HttpContext.Session.SetString("BFFSessionCookie", $"{cookieParts[0]}={cookieParts[1]}");
                            _httpContextAccessor.HttpContext.Session.SetString("serializedUser", JsonConvert.SerializeObject(userSession.User));
                            _httpContextAccessor.HttpContext.Session.SetString("sessionTokenValue", userSession.SessionToken.SessionTokenValue.ToString());

                            return SuccessResult(userSession.User.Email);
                        }
                    }
                    else if (storedCookie != null)
                    {
                        string[] cookiePartsStored = storedCookie.Split('=', 2);
                        string[] cookiePartsRequest = requestCookie.Split('=', 2);
                        _lastResponse = _backend.Get($"api/v2/Cookie", new { cookieName = cookiePartsStored[0], cookieValue = cookiePartsStored[1], siteFrom = "BFF" });
                        Cookie dbCookie = JsonConvert.DeserializeObject<Cookie>(_lastResponse.Response);
                        if (dbCookie != null)
                        {
                            dbCookie.CookieValue = cookiePartsRequest[1];
                            _lastResponse = _backend.Put("api/v2/Cookie", dbCookie);

                            _httpContextAccessor.HttpContext.Session.SetString("BFFSessionCookie", $"{cookiePartsRequest[0]}={cookiePartsRequest[1]}");
                            User authenticatedUser = JsonConvert.DeserializeObject<User>(serializedUser);

                            return SuccessResult(authenticatedUser.Email);
                        }
                    }
                }

                return AuthenticateResult.Fail("No session token");
            }
            catch (BackendException be)
            {
                return AuthenticateResult.Fail($"Failure to contact backend, returned message \"{be.Message}\"");
            } 
            catch (Exception e)
            {
                _logger.LogCritical(e.StackTrace);
                return AuthenticateResult.Fail("Validating authentication token failed due to: "+e.Message);
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