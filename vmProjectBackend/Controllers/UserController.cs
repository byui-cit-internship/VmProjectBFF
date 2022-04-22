using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vmProjectBackend.Models;
using Microsoft.AspNetCore.Authorization;
using vmProjectBackend.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserController> _logger;
        private BackendResponse _lastResponse;

        public UserController(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<UserController> logger)
        {
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _backend = new(_httpContextAccessor, _logger, _configuration);
            _auth = new(_backend, _logger);
        }

        /****************************************
        Create or update a user in the database to have admin privileges
        ****************************************/
        [HttpPost("admin/createuser")]
        public async Task<ActionResult<User>> PostAdminUser(PostAdmin postUser)
        {
            // Returns a admin user or null if email is not associated with an administrator
            User admin = _auth.getAuth("admin");

            if (admin != null)
            {
                // Get user object on the email provided by post
                _lastResponse = _backend.Get("api/v2/User", new { email = postUser.email });
                User user = JsonConvert.DeserializeObject<User>(_lastResponse.Response);

                // If user doesn't exist, create them with admin privileges
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
                // Else edit found user to be an admin
                else
                {
                    user.IsAdmin = true;

                    _lastResponse = _backend.Put("api/v2/User", user);

                    return Ok("modified user to be admin");
                }
            }
            return Unauthorized();
        }
    }
}
