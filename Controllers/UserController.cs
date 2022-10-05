using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using vmProjectBFF.Exceptions;
using vmProjectBFF.Models;

namespace vmProjectBFF.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BffController
    {

        public UserController(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<UserController> logger)
            : base(
                  configuration: configuration,
                  httpClientFactory: httpClientFactory,
                  httpContextAccessor: httpContextAccessor,
                  logger: logger)
        {
        }

        /***********************************************
         * This is for updating a user
         * If you are updating your self. 
         * ********************************************/

        [HttpPut("")]
        public async Task<ActionResult> PutUser(User user)
        {
            User admin = _auth.getAuth("admin");
            if (admin != null)
            {
                _lastResponse = _backend.Put("api/v2/User", user);
                User changedUser = JsonConvert.DeserializeObject<User>(_lastResponse.Response);

                if (admin.UserId == changedUser.UserId)
                {
                    return Ok(changedUser);
                }
                else
                {
                    return Ok("User was modified successfully");
                }

            }
            User authUser = _auth.getAuth("user");
            if (authUser != null)
            {
                if (user.UserId == authUser.UserId && user.IsAdmin == authUser.IsAdmin)
                {
                    _lastResponse = _backend.Put("api/v2/User", user);
                    User changedUser = JsonConvert.DeserializeObject<User>(_lastResponse.Response);

                    return Ok(changedUser);
                }
                else
                {
                    return BadRequest("Bad Request");
                }
            }


            return Unauthorized("Contact your admin to get access");


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
            catch (BffHttpException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }
        [HttpGet("professors")]
        public async Task<ActionResult> GetProfessors()
        {
            try
            {
                User professor = _auth.getAuth("admin");

                if (professor != null)
                {
                    BackendResponse userListResponse = _backend.Get($"api/v2/user");
                    List<User> userList = JsonConvert.DeserializeObject<List<User>>(userListResponse.Response);
                    List<User> professorList = userList.FindAll(p => p.IsAdmin == true);
                    return Ok(professorList);
                }
                else
                {
                    return Unauthorized("You are not a professor");
                }
            }
            catch (BackendException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }
    }
}
