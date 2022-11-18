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

        /**
         * <summary>
         * Returns a list of semesters, with  information of their start to end date.
         * </summary>
         * <returns>A list of "OldSectionDTO" objects repesenting information about sections the requesting professor teaches.</returns>
         * <remarks>
         * Only certain parameter combinations are allowed. Possible combinations include:<br/>
         * <![CDATA[
         *      <pre>
         *          <code>/api/semester/semester
         *          </code>
         *      </pre>
         * ]]>
         * Sample requests:
         *
         *      Returns the user logging in.
         *      GET /api/semester/semester
         *      RETURNS

         [
    {
        "semesterId": 9,
        "semesterYear": 2021,
        "semesterTerm": "Winter",
        "startDate": "2021-01-06T07:00:00",
        "endDate": "2021-04-24T06:00:00",
        "enrollmentTermCanvasId": 271
    }
]
         *     
         *
         * </remarks>
         * <response code="200">Returns a list of objects representing sections.</response>
         * <response code="403">Insufficent permission to make request.</response>
         */

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
    /**
         * <summary>
         * Returns a list of semesters, with  information of their start to end date.
         * </summary>
         * <returns>A list of "OldSectionDTO" objects repesenting information about sections the requesting professor teaches.</returns>
         * <remarks>
         * Only certain parameter combinations are allowed. Possible combinations include:<br/>
         * <![CDATA[
         *      <pre>
         *          <code>/api/semester/semester
         *          </code>
         *      </pre>
         * ]]>
         * Sample requests:
         *
         *      Returns the user logging in.
         *      GET /api/semester/semester
         *      RETURNS
         *     
         *
         * </remarks>
         * <response code="200">Returns a list of objects representing sections.</response>
         * <response code="403">Insufficent permission to make request.</response>
         */
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