using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VmProjectBFF.DTO;
using VmProjectBFF.DTO.Database;
using VmProjectBFF.DTO.Canvas;
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

        /****************************************
         * 
         ***************************************/
        /**
         * <summary>
         * Changes requesting user's approve status to "pending" and sets the role to "professor"
         * </summary>
         * <returns>Returns requesting user's information.</returns>
         * <remarks>
         * Only certain parameter combinations are allowed. Possible combinations include:<br/>
         *   
         * <![CDATA[
         *      <pre>
         *          <code>/api/user/requestAccess
         *          </code>
         *      </pre>
         * ]]>
         * Sample requests:
         *
         *      Returns the requesting user.
         *      PUT /api/user/requestAccess
         *      RETURNS
         *       {
         *           "userId": 1019,
         *           "firstName": "Joe",
         *           "lastName": "Doe",
         *           "email": "joedoe@byui.edu",
         *           "isAdmin": true,
         *           "canvasToken": "123456789asdfdfgh",
         *           "isVerified": true,
         *           "verificationCode": 0,
         *           "verificationCodeExpiration": "0001-01-01T00:00:00",
         *           "role": "professor",
         *           "approveStatus": "pending" 
         *        }
         *      
         *
         * </remarks>
         * <response code="200">Returns approved user.</response>
         * <response code="403">Insufficent permission to make request.</response>
         */
        [HttpPut("requestAccess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RequestAccess(User user)
        {
            try
            {
                CanvasUser canvasUser = _canvas.GetUserByCanvasToken(user.CanvasToken);
                User authUser = _authorization.GetAuth("user");

                if (authUser is not null &&
                    (authUser.approveStatus == "n/a" || authUser.approveStatus == "approved")
                    && user is not null
                    /*&& user.primary_email == canvasUser.primary_email */) // This won't work for testing
                {
                    authUser.role = "professor";
                    authUser.CanvasToken = user.CanvasToken;

                    if (authUser.approveStatus == "n/a")
                    {
                        authUser.approveStatus = "pending";
                        string message = "Your authorization request has been sent. An administrator will respond to your request.";
                        string subject = "Request sent.";
                        authUser = _backend.PutUser(authUser);
                        _emailClient.SendMessage(authUser.Email, subject, message);
                    }
                    else if (authUser.approveStatus == "approved")
                    {
                        authUser = _backend.PutUser(authUser);
                    }

                    return Ok(authUser);
                }
                return Forbid();
            }
            catch (BffHttpException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }
        /****************************************
         * 
         ***************************************/
        /**
         * <summary>
         * Changes user (with professor role) approved_status column to 'approved'
         * </summary>
         * <returns>Returns the approved professor.</returns>
         * <remarks>
         * Only certain parameter combinations are allowed. Possible combinations include:<br/>
         *   
         * <![CDATA[
         *      <pre>
         *          <code>/api/user/approve
         *          </code>
         *      </pre>
         * ]]>
         * Sample requests:
         *
         *      Returns the approved user.
         *      PUT /api/user/approve
         *      BODY
         *      {
         *          "email": "joedoe@byui.edu"
         *       }
         *      RETURNS
         *       {
         *           "userId": 1019,
         *           "firstName": "Joe",
         *           "lastName": "Doe",
         *           "email": "joedoe@byui.edu",
         *           "isAdmin": true,
         *           "canvasToken": "123456789asdfdfgh",
         *           "isVerified": true,
         *           "verificationCode": 0,
         *           "verificationCodeExpiration": "0001-01-01T00:00:00",
         *           "role": "professor",
         *           "approveStatus": "approved" 
         *        }
         *      
         *
         * </remarks>
         * <response code="200">Returns approved user.</response>
         * <response code="403">Insufficent permission to make request.</response>
         */
        [HttpPut("approve")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Approve([FromBody] User user)
        {
            User authUser = _authorization.GetAuth("admin");
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

        /*
        * <summary>
        * Sends current user's information.
        * </summary>
        * <return>Returns user's own information</returns>
        * 
        * <remarks>
        * <![CDATA[
        *   <pre>
        *       <code> /api/user/self </code>
        *   </pre>
        * ]]>
        *
        * Samples requests:
        *
        *   Returns the information of the user calling the endpoint.
        *   GET /api/user/self
        *   RETURNS 
        *       {
        *           "userId": 1019,
        *           "firstName": "Joe",
        *           "lastName": "Doe",
        *           "email": "joedoe@byui.edu",
        *           "isAdmin": true,
        *           "canvasToken": "123456789asdfdfgh",
        *           "isVerified": true,
        *           "verificationCode": 0,
        *           "verificationCodeExpiration": "0001-01-01T00:00:00",
        *           "role": "professor",
        *           "approveStatus": "approved" 
        *        }
        * </remarks>
        *
        * <response code="200">Returns user's information</response>
        * <response code="401">User not authenticated</response>
        * <response code="403">Insufficient permission to make request</response>
        */
        [HttpGet("self")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Self()
        {
            try
            {
                User authUser = _authorization.GetAuth("admin");
                if (authUser is not null)
                {
                    return Ok(authUser);
                }
                return Forbid();
            }
            catch (BffHttpException be)
            {
                return StatusCode((int)be.StatusCode, be.Message);
            }
        }

    }
}
