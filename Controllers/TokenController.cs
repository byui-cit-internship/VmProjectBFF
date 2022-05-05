using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using vmProjectBFF.DTO;
using vmProjectBFF.Models;
using vmProjectBFF.Services;

namespace vmProjectBFF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;
        private readonly Backend _backend;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private BackendResponse _lastResponse;

        public TokenController(
            ILogger<TokenController> logger,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _backend = new(_httpContextAccessor, _logger, _configuration);
            _httpClient = httpClientFactory.CreateClient();
        }

        /**************************************
        Validate the token given by the front end
        and then determines whether they are a teacher or professor
        ****************************************/
        [HttpPost()]
        [AllowAnonymous]
        public async Task<ActionResult> GetToken([FromBody] DTO.AccessTokenDTO accessTokenObj)
        {
            try
            {
                accessTokenObj.CookieName = ".VMProjectBFF.Session";
                accessTokenObj.CookieValue = _httpContextAccessor.HttpContext.Request.Cookies[accessTokenObj.CookieName];
                accessTokenObj.SiteFrom = "BFF";

                if (accessTokenObj.CookieValue == null)
                {
                    // return StatusCode(505, "Session cookie not set. Try again.");
                    accessTokenObj.CookieValue = Guid.NewGuid().ToString();
                    _httpContextAccessor.HttpContext.Response.Cookies.Append(accessTokenObj.CookieName, accessTokenObj.CookieValue, new CookieOptions(){SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict});
                }

                _lastResponse = _backend.Get("");
                _lastResponse = _backend.Post("api/v1/token", accessTokenObj);
                (User authenticatedUser, string sessionToken) authResult = JsonConvert.DeserializeObject<(User, string)>(_lastResponse.Response);

                _httpContextAccessor.HttpContext.Session.SetString("BFFSessionCookie", $"{accessTokenObj.CookieName}={accessTokenObj.CookieValue}");
                _httpContextAccessor.HttpContext.Session.SetString("serializedUser", JsonConvert.SerializeObject(authResult.authenticatedUser));
                _httpContextAccessor.HttpContext.Session.SetString("sessionTokenValue", authResult.sessionToken);
                return Ok(authResult.authenticatedUser);
            }
            catch (BackendException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);

            }
        }

        [HttpDelete()]
        public async Task<ActionResult> DeleteSession()
        {
            try
            {
                BackendResponse deleteResponse = _backend.Delete("api/v1/token", null);
                _httpContextAccessor.HttpContext.Session.Clear();
                _httpContextAccessor.HttpContext.Response.Cookies.Delete(".VMProjectBFF.Session");
                _httpContextAccessor.HttpContext.Response.Cookies.Delete(".VMProject.Session");
                return Ok();
            }
            catch (BackendException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }
    }
}
