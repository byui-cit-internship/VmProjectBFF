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

        [HttpPut("verifyUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> verifyUser([FromQuery] int code)
        {
            User authUser = _authorization.GetAuth("user");
            try
            {
                if (authUser.VerificationCodeExpiration > DateTime.Now && authUser.VerificationCode == code)
                {
                    authUser.IsVerified = true;
                    return Ok(_backend.PutUser(authUser));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (BffHttpException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }

        [HttpPut("sendCode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> SendCode()
        {
            User authUser = _authorization.GetAuth("user");

            if (authUser is not null)
            {
                int codeLength = 5;
                Random random = new();
                List<string> codeStr = new(codeLength);
                for (int i = 0; i < codeLength; i++)
                {
                    codeStr.Add(random.Next(1, 9).ToString());
                }
                int code = int.Parse(string.Concat(codeStr));

                DateTime codeExpDate = DateTime.Now.AddDays(1);

                authUser.VerificationCode = code;
                authUser.VerificationCodeExpiration = codeExpDate;

                try
                {
                    authUser = _backend.PutUser(authUser);
                    _emailClient.SendCode(authUser.Email, code.ToString(), "Vima Confirmation Code");

                    authUser.VerificationCode = null;
                    return Ok(authUser);
                }
                catch (BffHttpException be)
                {
                    return StatusCode((int)be.StatusCode, be.Message);
                }
            }
            return Forbid();
        }

        [HttpPut("requestAccess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RequestAccess()
        {
            User authUser = _authorization.GetAuth("user"); // admin should not be linked to role
            if (authUser is not null && authUser.approveStatus == "n/a")
            {
                try
                {
                    authUser.approveStatus = "pending";
                    authUser.role = "professor";
                    authUser = _backend.PutUser(authUser);

                    string message = "Your authorization request has been sent. An administrator will respond to your request.";
                    _emailClient.SendMessage(authUser.Email, "authorization request", message);

                    return Ok(authUser);
                }
                catch (BffHttpException be)
                {
                    return StatusCode((int)be.StatusCode, be.Message);
                }
            }
            return Forbid();
        }

        [HttpPut("approveAccess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> ApproveAccess([FromBody] User user) // you only have pass the field you need...
        {
            User authUser = _authorization.GetAuth("admin"); // admin should not be linked to role
            if (authUser is not null)
            {
                try
                {   
                    user = _backend.GetUserByEmail(user.Email);
                    user.approveStatus = "approved";

                    user = _backend.PutUser(user);
                    _emailClient.SendMessage(user.Email, "Request approved", "Your request has been approved, you can now login.");

                    return Ok(user);
                }
                catch (BffHttpException be)
                {
                    return StatusCode((int)be.StatusCode, be.Message);
                }
            }
            return Forbid();
        }
    }
}
