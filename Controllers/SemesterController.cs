using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VmProjectBFF.DTO.Database;
using VmProjectBFF.Exceptions;
using VmProjectBFF.Services;

namespace VmProjectBFF.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterController : BffController
    {
        public SemesterController(
            IAuthorization authorization,
            IBackendRepository backend,
            ICanvasRepository canvas,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<SemesterController> logger,
            IVCenterRepository vCenter)
            : base(
                  authorization: authorization,
                  backend: backend,
                  canvas: canvas,
                  configuration: configuration,
                  httpClientFactory: httpClientFactory,
                  httpContextAccessor: httpContextAccessor,
                  logger: logger,
                  vCenter: vCenter)
        {
        }

        /****************************************
         * Return A list of all semesters
         ***************************************/

        [HttpGet("semester")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetSemester()
        {
            try
            {
                User professor = _authorization.GetAuth("admin");

                if (professor is not null)
                {
                    dynamic thing = _backend.GetAllSemesters(professor.UserId);
                    return Ok(thing);
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

        /****************************************
         * Return A list of all enrollment terms from canvas
         ***************************************/

        [HttpGet("enrollmentTerms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetEnrollmentTerms()
        {
            try
            {
                User professor = _authorization.GetAuth("admin");

                if (professor is not null)
                {
                    return Ok(_canvas.GetEnrollmentTerms(professor.CanvasToken));
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