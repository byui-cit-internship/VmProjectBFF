using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;

namespace vmProjectBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public IHttpClientFactory _httpClientFactory { get; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        ILogger Logger { get; } = AppLogger.CreateLogger<TokenController>();
        public const string SessionKeyName = "_Name";
        public const string SessionKeyId = "_Id";

        public TokenController(DatabaseContext context, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
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
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {accessTokenObj.AccessTokenValue}");
                HttpResponseMessage response = await httpClient.GetAsync($"https://www.googleapis.com/userinfo/v2/me");

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    dynamic students = JsonConvert.DeserializeObject<dynamic>(responseString);

                    string email = students.email;

                    User user = (from u in _context.Users
                                 where u.Email == email
                                 select u).FirstOrDefault();

                    if (user == null)
                    {

                        user = new User();
                        user.Email = email;
                        user.FirstName = students.given_name;
                        user.LastName = students.family_name;
                        user.IsAdmin = false;

                        _context.Users.Add(user); ;
                        _context.SaveChanges();

                    }

                    AccessToken accessToken = (from at in _context.AccessTokens
                                               where at.AccessTokenValue == accessTokenObj.AccessTokenValue
                                               select at).FirstOrDefault();
                    if (accessToken == null)
                    {
                        accessToken = new();
                        accessToken.AccessTokenValue = accessTokenObj.AccessTokenValue;
                        accessToken.ExpireDate = DateTime.Now.AddHours(1);
                        accessToken.User = user;

                        _context.AccessTokens.Add(accessToken);
                        _context.SaveChanges();
                    }
                    else if (DateTime.Compare(accessToken.ExpireDate, DateTime.Now) < 0)
                    {
                        return Forbid();
                    }


                    SessionToken sessionToken = (from st in _context.SessionTokens
                                                 where st.AccessToken == accessToken
                                                 orderby st.ExpireDate descending
                                                 select st).FirstOrDefault();
                    if (sessionToken == null)
                    {
                        sessionToken = new();
                        sessionToken.SessionTokenValue = sessionToken.SessionTokenValue = Guid.NewGuid();
                        sessionToken.SessionCookie = _httpContextAccessor.HttpContext.Request.Cookies[".VMProject.Session"];
                        sessionToken.AccessToken = accessToken;
                        sessionToken.ExpireDate = DateTime.Now.AddDays(3);

                        _context.SessionTokens.Add(sessionToken);
                        _context.SaveChanges();
                    }
                    else if (DateTime.Compare(sessionToken.ExpireDate, DateTime.Now) < 0)
                    {
                        return Forbid();
                    }

                    
                    // outside return statment
                    return Ok(user);
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500);

            }
        }

        [HttpDelete()]
        public async Task<ActionResult> DeleteSession()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(".VMProject.Session");
            return Ok();
        }
    }
}
