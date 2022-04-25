using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vmProjectBackend.DAL;
using vmProjectBackend.Models;
using Microsoft.AspNetCore.Authorization;
using vmProjectBackend.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using vmProjectBackend.DTO;
using Newtonsoft.Json;

namespace vmProjectBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Authorization _auth;
        private readonly Backend _backend;
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserController> _logger;
        private BackendResponse _lastResponse;

        public UserController(
            DatabaseContext context,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _backend = new(_httpContextAccessor, _logger, _configuration);
            _auth = new(_backend, _context, _logger);
        }

        /****************************************
        Create or update a user in the database to have admin privileges
        ****************************************/
        [HttpPost("admin/createuser")]
        public async Task<ActionResult<User>> PostAdminUser(PostAdmin postUser)
        {
            try
            {
                User admin = _auth.getAuth("admin");

                if (admin != null)
                {
                    _lastResponse = _backend.Get("api/v2/User", new { email = postUser.email });
                    User user = JsonConvert.DeserializeObject<User>(_lastResponse.Response);

                    if (user == null)
                    {
                        user = new User();
                        user.Email = postUser.email;
                        user.FirstName = postUser.firstName;
                        user.LastName = postUser.lastName;
                        user.IsAdmin = true;

                        _lastResponse = _backend.Post("api/v2/User", user);

                        return Ok("created user as admin");
                    }
                    else
                    {
                        user.IsAdmin = true;

                        _lastResponse = _backend.Put("api/v2/User", user);

                        return Ok("modified user to be admin");
                    }
                }
                return Unauthorized();
            }
            catch (BackendException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }
    }
}
