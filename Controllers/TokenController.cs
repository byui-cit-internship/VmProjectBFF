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
        /**
         * <summary>
         * Authenticates user. 
         * </summary>
         * <returns>A "User" object representing the user who logged in</returns>
         * <remarks>
         * Only certain parameter combinations are allowed. Possible combinations include:<br/>
         * <![CDATA[
         *      <pre>
         *          <code>/api/token
         *          </code>
         *      </pre>
         * ]]>
         * Sample requests:
         *
         *      Returns the user logging in.
         *      POST /api/token
         *      BODY 
         *      {
         *          "AccessTokenValue": "{accessTokenValue}"
         *      }
         *      RETURNS
         *      {
         *          "userId": 1,
         *          "firstName": "Michael",
         *          "lastName": "Ebenal",
         *          "email": "ebe17003@byui.edu",
         *          "is_admin": true,
         *          "canvas_token": null
         *      }
         *
         * A <c>510</c> error code signifies that the <c>.VMProjectBFF.Session</c> cookie is not set.
         * If before sending a request to this endpoint the cookies are checked and this cookie
         * does exist, check it is being sent in the headers of the request. Requests can be
         * seen in their entirety in the Network tab of Chrome Dev Tools. If the cookie can be
         * verified as being present in the cookies store and is in the headers in the request
         * to the BFF to this endpoint, escalate the issue immediately so it may be resolved.
         * If the cookie does not exist prior to sending a request to this endpoint, the fix is
         * to send any request to the BFF application, which should set the cookie. The liveprobe
         * endpoint is a great candidate for an empty GET request which will set the cookie. If the
         * cookie is still not being set, generally that has to do with the conditions under which
         * the first "empty" request was sent. Some combinations of headers do not allow the
         * responding application (the BFF) to set cookies, which will cause problems.
         *
         * </remarks>
         * <response code="200">Returns a user object representing the person logged in</response>
         * <response code="500">Server side error</response>
         * <response code="510">Cookie not set upon recieving request. NYI</response>
         */
        [HttpPost()]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(510)]
        public async Task<ActionResult> GetToken([FromBody] DTO.AccessTokenDTO accessTokenObj)
        {
            try
            {
                accessTokenObj.CookieName = ".VMProjectBFF.Session";
                accessTokenObj.CookieValue = _httpContextAccessor.HttpContext.Request.Cookies[accessTokenObj.CookieName];
                accessTokenObj.SiteFrom = "BFF";

                if (accessTokenObj.CookieValue == null)
                {
                    // return StatusCode(510, "Session cookie not set. Try again.");
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

        /**
         * <summary>
         * Logs out user. 
         * </summary>
         * <returns>An empty response with status code 200</returns>
         * <remarks>
         * Sample requests:
         *
         *      Returns the user logging in.
         *      DELETE /api/token
         *      RETURNS
         *      {
         *      }
         *
         * </remarks>
         * <response code="200">Signifies a user has been successfully signed out</response>
         */
        [HttpDelete()]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

// C:\Users\Dell\Documents\GitHub\VmProjectBFF\Controllers\TokenController.cs