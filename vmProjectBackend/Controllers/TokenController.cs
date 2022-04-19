using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using vmProjectBackend.DAL;
using vmProjectBackend.DTO;
using vmProjectBackend.Models;
using vmProjectBackend.Services;

namespace vmProjectBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<TokenController> _logger;
        private readonly Backend _backend;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private BackendResponse _lastResponse;

        public TokenController(
            DatabaseContext context,
            ILogger<TokenController> logger,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _context = context;
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
        public async Task<ActionResult> GetToken([FromBody] DTO.AccessToken accessTokenObj)
        {
            try
            {
                accessTokenObj.CookieName = ".VMProjectBFF.Session";
                accessTokenObj.CookieValue = _httpContextAccessor.HttpContext.Request.Cookies[accessTokenObj.CookieName];
                accessTokenObj.SiteFrom = "BFF";

                if (accessTokenObj.CookieValue == null)
                {
                    return StatusCode(505, "Session cookie not set. Try again.");
                }

                _lastResponse = _backend.Get("");
                string cookie;
                if (_lastResponse.HttpResponse.Headers.Contains("Set-Cookie"))
                {
                    string cookieHeader = _lastResponse.HttpResponse.Headers.GetValues("Set-Cookie")?.ToArray()[0];
                    if (cookieHeader == null)
                    {
                        return StatusCode(505, "Cookie not recieved on success");
                    }
                    cookie = cookieHeader.Split(';', 2)[0];
                }
                else
                {
                    {
                        return StatusCode(505, "Cookie not recieved on success");
                    }
                }

                _lastResponse = _backend.Post("api/v1/token", accessTokenObj);
                _httpContextAccessor.HttpContext.Session.SetString("BESessionCookie", cookie);
                _httpContextAccessor.HttpContext.Session.SetString("BFFSessionCookie", $"{accessTokenObj.CookieName}={accessTokenObj.CookieValue}");
                (User authenticatedUser, string sessionToken) authResult = JsonConvert.DeserializeObject<(User, string)>(_lastResponse.Response);
                _httpContextAccessor.HttpContext.Session.SetString("serializedUser", JsonConvert.SerializeObject(authResult.authenticatedUser));
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
                _httpContextAccessor.HttpContext.Response.Cookies.Delete(".VmProjectBFF.Session");
                return Ok();
            }
            catch (BackendException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }
    }
}
