using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VmProjectBFF.DTO;
using VmProjectBFF.DTO.Database;
using VmProjectBFF.Exceptions;
using VmProjectBFF.Services;

namespace VmProjectBFF.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BffController
    {
        private readonly IEmailClient _emailClient;
        public UserController(
            IAuthorization authorization,
            IBackendRepository backend,
            ICanvasRepository canvas,
            IConfiguration configuration,
            IEmailClient emailClient,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<UserController> logger,
            IVCenterRepository vCenter)
            : base(
                  authorization: authorization,
                  backend: backend,
                  canvas: canvas,
                  configuration: configuration,
                  httpClientFactory: httpClientFactory,
                  httpContextAccessor: httpContextAccessor,
                  logger: logger,
                  vCenter: vCenter
                  )
                
        {
            _emailClient = emailClient;
        }

        /***********************************************
         * This is for updating a user
         * If you are updating your self. 
         * ********************************************/

        [HttpPut("")]
        public async Task<ActionResult> PutUser(User user)
        {
            User admin = _authorization.GetAuth("admin");
            if (admin is not null)
            {
                return Ok(_backend.PutUser(user));

            }
            User authUser = _authorization.GetAuth("user");
            if (authUser is not null)
            {
                if (user.UserId == authUser.UserId && user.IsAdmin == authUser.IsAdmin)
                {
                    return Ok(_backend.PutUser(user));
                }
                else
                {
                    return BadRequest("Bad Request");
                }
            }
            return Forbid();
        }

        /****************************************
        Create or update a user in the database to have admin privileges
        ****************************************/
        [HttpPost("admin/createuser")]
        public async Task<ActionResult<User>> PostAdminUser(PostAdmin postUser)
        {
            try
            {   
                User admin = _authorization.GetAuth("admin");

                if (admin is not null)
                {
                    User user = _backend.GetUserByEmail(postUser.email);

                    if (user is null)
                    {
                        user = new()
                        {
                            Email = postUser.email,
                            FirstName = postUser.firstName,
                            LastName = postUser.lastName,
                            IsAdmin = true
                        };

                        return Ok(_backend.PostUser(user));
                    }
                    else
                    {
                        user.IsAdmin = true;
                        return Ok(_backend.PutUser(user));
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
                User professor = _authorization.GetAuth("admin");

                if (professor is not null)
                {
                    return Ok(_backend.GetAdmins());
                }
                else
                {
                    return Forbid();
                }
            }
            catch (BffHttpException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }

        [HttpGet("bySection")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetUsersByCourse(int sectionId)
        {
            try
            {
                User professor = _authorization.GetAuth("admin");

                if (professor is not null)
                {
                    return Ok(_backend.GetUsersBySection(sectionId));
                }
                else
                {
                    return Forbid();
                }
            }
            catch (BffHttpException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }
    }
}
